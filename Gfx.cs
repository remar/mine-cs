using System;
using static SDL2.SDL;

class Gfx {
    private struct Tile {
	public SDL_Rect src;
	public SDL_Rect dest;
	public Tile(int tile, int x, int y) {
	    src = MakeRect(tile * cellWidth, 0, cellWidth, cellHeight);
	    dest = MakeRect(x * cellWidth, y * cellHeight,
			    cellWidth, cellHeight);
	}
    }

    const int cellWidth = 32;
    const int cellHeight = 32;
    public int rows {get; set;}
    public int cols {get; set;}
    private IntPtr Renderer = IntPtr.Zero;
    private IntPtr gfx = IntPtr.Zero;
    private Tile[,] field;

    public Gfx(int rows = 10, int cols = 10) {
	this.rows = rows;
	this.cols = cols;
	field = new Tile[rows, cols];
	for(int y = 0;y < rows;y++) {
	    for(int x = 0;x < cols;x++) {
		field[y,x] = new Tile(10, x, y);
	    }
	}
    }

    public void Init() {
	IntPtr Window = IntPtr.Zero;
	IntPtr img = IntPtr.Zero;
	
	Window = SDL_CreateWindow ("Minesweeper!",
				   SDL_WINDOWPOS_UNDEFINED,
				   SDL_WINDOWPOS_UNDEFINED,
				   cols * cellWidth, // Width
				   rows * cellHeight, // Height
				   SDL_WindowFlags.SDL_WINDOW_SHOWN);

	Renderer =
	    SDL_CreateRenderer(Window,
			       -1,
			       SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

	SDL_SetRenderDrawColor (Renderer, 0xae, 0xFF, 0x00, 0xFF);
	img = SDL_LoadBMP("gfx.bmp");
	gfx = SDL_CreateTextureFromSurface(Renderer, img);
	SDL_FreeSurface(img);
    }

    public void Render() {
	SDL_RenderClear (Renderer);
	for(int y = 0;y < rows;y++) {
	    for(int x = 0;x < cols;x++) {
		SDL_RenderCopy(Renderer, gfx,
			       ref field[y,x].src,
			       ref field[y,x].dest);
	    }
	}
	SDL_RenderPresent (Renderer);
    }

    public void SetTile(int tile, int x, int y) {
	field[y,x].src.x = tile * cellWidth;
    }

    private static SDL_Rect MakeRect(int x, int y, int w, int h) {
	SDL_Rect rect = new SDL_Rect();
	rect.x = x;
	rect.y = y;
	rect.w = w;
	rect.h = h;
	return rect;
    }
}
