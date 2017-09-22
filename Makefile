all:
	dmcs -r:SDL2-CS.dll Mine.cs Gfx.cs Field.cs

clean:
	rm Mine.exe* gfx.bmp
