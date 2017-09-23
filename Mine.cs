using System;
using SDL2;

class Mine {
    public static int Main(string[] args) {
	int rows = 15;
	int cols = 15;
	int mines = 30;

	if(args.Length > 0) {
	    try {
		if(args.Length == 1) {
		    int dim = int.Parse(args[0]);
		    rows = cols = dim;
		} else if(args.Length == 2) {
		    cols = int.Parse(args[0]);
		    rows = int.Parse(args[1]);
		} else {
		    Console.WriteLine("Usage: Mine.exe <width> <height>");
		    return 0;
		}
		mines = (rows * cols) / 6;
	    } catch(Exception) {
		Console.WriteLine("Usage: Mine.exe <width> <height>");
		return 0;
	    }
	}

	SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
	Gfx gfx = new Gfx(rows, cols);
	Field field = new Field(gfx);

	gfx.Init();

	field.AddMines(mines);

	SDL.SDL_Event Event;
	bool Running = true;
	while(Running) {
	    while(SDL.SDL_PollEvent(out Event) != 0) {
		if(Event.type == SDL.SDL_EventType.SDL_QUIT) {
		    Running = false;
		} else if(Event.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN) {
		    if(field.GameOver()) {
			continue;
		    }
		    int x = Event.button.x / 32;
		    int y = Event.button.y / 32;
		    if(Event.button.button == SDL.SDL_BUTTON_LEFT) {
			field.Reveal(x, y);
		    } else if(Event.button.button == SDL.SDL_BUTTON_RIGHT) {
			field.Flag(x, y);
		    }
		} else if(Event.type == SDL.SDL_EventType.SDL_KEYDOWN) {
		    if(Event.key.keysym.sym == SDL.SDL_Keycode.SDLK_r) {
			field.Reset();
		    }
		}
	    }
	    gfx.Render();
	}

	SDL.SDL_Quit();

	return 0;
    }
}
