using System;
using static SDL2.SDL;

class Mine {
    public static int Main() {
	SDL_Init(SDL_INIT_VIDEO);
	int rows = 15;
	int cols = 15;

	Gfx gfx = new Gfx(rows, cols);
	Field field = new Field(gfx);

	gfx.Init();

	field.AddMines(20);

	SDL_Event Event;
	bool Running = true;
	while(Running) {
	    while(SDL_PollEvent(out Event) != 0) {
		if(Event.type == SDL_EventType.SDL_QUIT) {
		    Running = false;
		} else if(Event.type == SDL_EventType.SDL_MOUSEBUTTONDOWN) {
		    int x = Event.button.x / 32;
		    int y = Event.button.y / 32;
		    field.Reveal(x, y);
		}
	    }
	    gfx.Render();
	}

	SDL_Quit();

	return 0;
    }
}
