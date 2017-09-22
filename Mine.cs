using System;
using SDL2;

class Mine {
    public static int Main() {
	SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
	int rows = 15;
	int cols = 15;
	int mines = 30;

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
		    int x = Event.button.x / 32;
		    int y = Event.button.y / 32;
		    if(Event.button.button == SDL.SDL_BUTTON_LEFT) {
			field.Reveal(x, y);
		    } else if(Event.button.button == SDL.SDL_BUTTON_RIGHT) {
			field.Flag(x, y);
		    }
		}
	    }
	    gfx.Render();
	}

	SDL.SDL_Quit();

	return 0;
    }
}
