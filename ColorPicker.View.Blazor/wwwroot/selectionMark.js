/* ============================
   STATE
============================ */

const markerStates = new WeakMap();

/* ============================
   PUBLIC API
============================ */

export function initSelectionMarker(canvas, value, dotNetRef) {
    if (!canvas) return;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    markerStates.set(canvas, {
        value: clamp(value),
        dotNetRef,
        pointerId: null
    });

    function redraw() {
        const rect = canvas.getBoundingClientRect();
        const dpr = window.devicePixelRatio || 1;

        const width = Math.round(rect.width * dpr);
        const height = Math.round(rect.height * dpr);

        if (canvas.width !== width || canvas.height !== height) {
            canvas.width = width;
            canvas.height = height;
        }

        ctx.setTransform(dpr, 0, 0, dpr, 0, 0);

        drawMarker(ctx, rect.width, rect.height, markerStates.get(canvas).value);
    }

    redraw();

    const resizeObserver = new ResizeObserver(redraw);
    resizeObserver.observe(canvas);

    markerStates.get(canvas).resizeObserver = resizeObserver;

    // Pointer events
    canvas.addEventListener("pointerdown", e => onPointerDown(e, canvas));
    canvas.addEventListener("pointermove", e => onPointerMove(e, canvas));
    canvas.addEventListener("pointerup", e => onPointerUp(e, canvas));
    canvas.addEventListener("pointercancel", e => onPointerUp(e, canvas));
}

export function updateSelectionMarker(canvas, value) {
    if (!canvas) return;

    const state = markerStates.get(canvas);
    if (!state) return;

    state.value = clamp(value);

    redrawOnlyMarker(canvas);
}

/* ============================
   POINTER HANDLING
============================ */

function onPointerDown(e, canvas) {
    const state = markerStates.get(canvas);
    if (!state) return;

    canvas.setPointerCapture(e.pointerId);
    state.pointerId = e.pointerId;

    updateFromPointer(e, canvas);
}

function onPointerMove(e, canvas) {
    const state = markerStates.get(canvas);
    if (!state || state.pointerId !== e.pointerId) return;

    updateFromPointer(e, canvas);
}

function onPointerUp(e, canvas) {
    const state = markerStates.get(canvas);
    if (!state) return;

    try {
        canvas.releasePointerCapture(e.pointerId);
    } catch { }

    state.pointerId = null;
}

/* ============================
   CORE UPDATE
============================ */

function updateFromPointer(e, canvas) {
    const rect = canvas.getBoundingClientRect();
    const horizontal = rect.width >= rect.height;

    let value = horizontal
        ? (e.clientX - rect.left) / rect.width
        : (e.clientY - rect.top) / rect.height;

    value = clamp(value);

    const state = markerStates.get(canvas);
    if (!state) return;

    state.value = value;

    redrawOnlyMarker(canvas);

    state.dotNetRef?.invokeMethodAsync("OnValueChanged", value);
}

/* ============================
   DRAWING
============================ */

function redrawOnlyMarker(canvas) {
    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    const rect = canvas.getBoundingClientRect();

    drawMarker(ctx, rect.width, rect.height, markerStates.get(canvas).value);
}

function drawMarker(ctx, width, height, value) {
    ctx.clearRect(0, 0, width, height);

    const horizontal = width >= height;

    const size = Math.min(width / 3, height / 3);

    ctx.save();
    ctx.fillStyle = "#000";

    if (horizontal) {
        const x = value * width;

        ctx.beginPath();
        ctx.moveTo(x, size);
        ctx.lineTo(x - size, 0);
        ctx.lineTo(x + size, 0);
        ctx.closePath();
        ctx.fill();

        ctx.beginPath();
        ctx.moveTo(x, height - size);
        ctx.lineTo(x - size, height);
        ctx.lineTo(x + size, height);
        ctx.closePath();
        ctx.fill();
    } else {
        const y = value * height;

        ctx.beginPath();
        ctx.moveTo(size, y);
        ctx.lineTo(0, y - size);
        ctx.lineTo(0, y + size);
        ctx.closePath();
        ctx.fill();

        ctx.beginPath();
        ctx.moveTo(width - size, y);
        ctx.lineTo(width, y - size);
        ctx.lineTo(width, y + size);
        ctx.closePath();
        ctx.fill();
    }

    ctx.restore();
}

/* ============================
   HELPERS
============================ */

function clamp(v) {
    return Math.max(0, Math.min(1, v));
}