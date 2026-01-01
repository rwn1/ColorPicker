/* ============================
   PUBLIC API
============================ */

export function initSelectedColorForeground(canvas, color) {
    if (!canvas) return;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    canvas._selectedColor = color;

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

        drawSelectedColor(ctx, rect.width, rect.height, canvas._selectedColor);
    }

    redraw();

    const resizeObserver = new ResizeObserver(redraw);
    resizeObserver.observe(canvas);

    canvas._selectedColorObserver = resizeObserver;
}

export function updateSelectedColorForeground(canvas, color) {
    if (!canvas) return;

    canvas._selectedColor = color;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    const rect = canvas.getBoundingClientRect();
    drawSelectedColor(ctx, rect.width, rect.height, color);
}

/* ============================
   DRAWING
============================ */

function drawSelectedColor(ctx, width, height, argb) {
    ctx.clearRect(0, 0, width, height);

    const rgba = argbToRgba(argb);

    ctx.fillStyle = rgba;
    ctx.fillRect(0, 0, width, height);
}

/* ============================
   COLOR HELPERS
============================ */

function argbToRgba(argb) {
    if (!argb || typeof argb !== "string") {
        return "rgba(0,0,0,1)";
    }

    // #AARRGGBB
    if (argb.startsWith("#") && argb.length === 9) {
        const a = parseInt(argb.slice(1, 3), 16) / 255;
        const r = parseInt(argb.slice(3, 5), 16);
        const g = parseInt(argb.slice(5, 7), 16);
        const b = parseInt(argb.slice(7, 9), 16);

        return `rgba(${r}, ${g}, ${b}, ${a})`;
    }

    // fallback #RRGGBB
    if (argb.startsWith("#") && argb.length === 7) {
        return argb;
    }

    return argb;
}