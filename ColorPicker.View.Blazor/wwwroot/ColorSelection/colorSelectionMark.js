/* ============================
   STATE
============================ */

const svStates = new WeakMap();

/* ============================
   PUBLIC API
============================ */

export function initColorSelectionMarker(canvas, s, v, dotNetRef) {
    if (!canvas) return;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    svStates.set(canvas, {
        s: clamp(s),
        v: clamp(v),
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

        drawMarker(ctx, rect.width, rect.height, svStates.get(canvas));
    }

    redraw();

    const ro = new ResizeObserver(redraw);
    ro.observe(canvas);

    svStates.get(canvas).resizeObserver = ro;

    // pointer events
    canvas.addEventListener("pointerdown", e => onPointerDown(e, canvas));
    canvas.addEventListener("pointermove", e => onPointerMove(e, canvas));
    canvas.addEventListener("pointerup", e => onPointerUp(e, canvas));
    canvas.addEventListener("pointercancel", e => onPointerUp(e, canvas));
}

export function updateColorSelectionMarker(canvas, s, v) {
    if (!canvas) return;

    const state = svStates.get(canvas);
    if (!state) return;

    state.s = clamp(s);
    state.v = clamp(v);

    redrawOnly(canvas);
}

/* ============================
   POINTER HANDLING
============================ */

function onPointerDown(e, canvas) {
    const state = svStates.get(canvas);
    if (!state) return;

    canvas.setPointerCapture(e.pointerId);
    state.pointerId = e.pointerId;

    updateFromPointer(e, canvas);
}

function onPointerMove(e, canvas) {
    const state = svStates.get(canvas);
    if (!state || state.pointerId !== e.pointerId) return;

    updateFromPointer(e, canvas);
}

function onPointerUp(e, canvas) {
    const state = svStates.get(canvas);
    if (!state) return;

    try {
        canvas.releasePointerCapture(e.pointerId);
    } catch { }

    state.pointerId = null;
}

/* ============================
   CORE LOGIC
============================ */

function updateFromPointer(e, canvas) {
    const rect = canvas.getBoundingClientRect();

    const x = clamp((e.clientX - rect.left) / rect.width);
    const y = clamp((e.clientY - rect.top) / rect.height);

    const state = svStates.get(canvas);
    if (!state) return;

    state.s = x;
    state.v = y;

    redrawOnly(canvas);

    state.dotNetRef?.invokeMethodAsync("OnValueChanged", state.s, (1 - state.v));
}

/* ============================
   DRAWING
============================ */

function redrawOnly(canvas) {
    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    const rect = canvas.getBoundingClientRect();
    drawMarker(ctx, rect.width, rect.height, svStates.get(canvas));
}

function drawMarker(ctx, width, height, state) {
    ctx.clearRect(0, 0, width, height);

    const x = state.s * width;
    const y = state.v * height;

    const radius = 3;

    ctx.save();

    ctx.beginPath();
    ctx.arc(x, y, radius + 2, 0, Math.PI * 2);
    ctx.strokeStyle = "#fff";
    ctx.lineWidth = 1;
    ctx.stroke();

    ctx.beginPath();
    ctx.arc(x, y, radius + 1, 0, Math.PI * 2);
    ctx.strokeStyle = "#000";
    ctx.lineWidth = 1;
    ctx.stroke();

    ctx.beginPath();
    ctx.arc(x, y, radius, 0, Math.PI * 2);
    ctx.strokeStyle = "#fff";
    ctx.lineWidth = 1;
    ctx.stroke();

    ctx.restore();
}

/* ============================
   HELPERS
============================ */

function clamp(v) {
    return Math.max(0, Math.min(1, v));
}