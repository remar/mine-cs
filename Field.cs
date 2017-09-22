using System;

class Field {
    enum CellType {Empty, Mine};
    enum Visibility {Hidden, Revealed};

    private Gfx gfx;
    private CellType[,] cells;
    private Visibility[,] visibility;
    private bool[,] visited;
    private bool[,] flagged;
    private int width {get {return cells.GetLength(0);}}
    private int height {get {return cells.GetLength(1);}}
    private int[] offsetX = {-1, 0, 1, -1, 1, -1, 0, 1};
    private int[] offsetY = {-1, -1, -1, 0, 0, 1, 1, 1};

    public Field(Gfx gfx) {
	this.gfx = gfx;
	cells = new CellType[gfx.cols, gfx.rows];
	visibility = new Visibility[gfx.cols, gfx.rows];
	visited = new bool[width, height];
	flagged = new bool[width, height];
	for(int y = 0;y < height;y++) {
	    for(int x = 0;x < width;x++) {
		cells[x,y] = CellType.Empty;
		visibility[x,y] = Visibility.Hidden;
	    }
	}
    }

    public void AddMines(int amount) {
	int[] positions = new int[width*height];
	for(int i = 0;i < positions.Length;i++) {
	    positions[i] = i;
	}
	Shuffle(positions);
	for(int i = 0;i < amount;i++) {
	    int x = positions[i] % width;
	    int y = positions[i] / width;
	    cells[x,y] = CellType.Mine;
	}
    }

    public void Reveal(int x, int y) {
	if(visibility[x,y] == Visibility.Revealed) {
	    return;
	}

	visibility[x,y] = Visibility.Revealed;

	if(cells[x,y] == CellType.Mine) {
	    Console.WriteLine("!!!BOOOOOOM!!!");
	    gfx.SetTile(11, x, y);
	} else {
	    StartRecursiveReveal(x, y);
	}
    }

    public void Flag(int x, int y) {
	if(visibility[x,y] == Visibility.Revealed) {
	    return;
	}

	if(flagged[x,y]) {
	    gfx.SetTile(10, x, y);
	} else {
	    gfx.SetTile(9, x, y);
	}

	flagged[x,y] = !flagged[x,y];
    }

    private void StartRecursiveReveal(int x, int y) {
	ClearVisited();
	RecursiveReveal(x, y);
    }

    private void ClearVisited() {
	for(int y = 0;y < height;y++) {
	    for(int x = 0;x < width;x++) {
		visited[x, y] = false;
	    }
	}
    }
    
    private void RecursiveReveal(int x, int y) {
	if(x < 0 || x >= width || y < 0 || y >= height || visited[x,y]) {
	    return;
	}
	visited[x, y] = true;
	int count = GetMineCount(x, y);
	gfx.SetTile(count, x, y);
	visibility[x,y] = Visibility.Revealed;
	if(count == 0) {
	    for(int i = 0;i < offsetX.Length;i++) {
		RecursiveReveal(x + offsetX[i], y + offsetY[i]);
	    }
	}
    }

    private int GetMineCount(int x, int y) {
	int count = 0;
	for(int i = 0;i < offsetX.Length;i++) {
	    count += (MineAt(x + offsetX[i], y + offsetY[i]) ? 1 : 0);
	}
	return count;
    }

    private bool MineAt(int x, int y) {
	if(x < 0 || x >= width
	   || y < 0 || y >= height) {
	    return false;
	} else {
	    return cells[x,y] == CellType.Mine;
	}
    }

    private void Shuffle(int[] arr) {
	Random rng = new Random();
	int n = arr.Length;
        while (n > 1) 
        {
            int k = rng.Next(n--);
            int temp = arr[n];
            arr[n] = arr[k];
            arr[k] = temp;
        }
    }
}
