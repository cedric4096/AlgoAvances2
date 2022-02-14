using Backtrack;

// Global arrays for validity memorisation
bool[,] existsOnLine = new bool[9, 9];
bool[,] existsOnColumn = new bool[9, 9];
bool[,] existsInBlock = new bool[9, 9];

// Global list of empty cells in grid, contains cells sorted by number of possible values
List<EmptyCell> positions = new List<EmptyCell>();

/// <summary>
/// Recursively checks if the grid is valid
/// </summary>
bool IsValid(int[,] grid, int position)
{
	if (position >= positions.Count || positions[position] == null) // If no more cells, valid branch
		return true;

	int j = positions[position].X, i = positions[position].Y; // Gets the coordinates of the cell in the grid

	for (int k = 0; k < 9; k++)
	{
		// For each value, check if it doesn't exist
		if (!existsOnLine[i, k] && !existsOnColumn[j, k] && !existsInBlock[3 * (i / 3) + (j / 3), k])
		{
			// Add k to saved values
			existsOnLine[i, k] = existsOnColumn[j, k] = existsInBlock[3 * (i / 3) + (j / 3), k] = true;

			if (IsValid(grid, position + 1))
			{
				// Saves the valid choice in the grid
				grid[i, j] = k + 1;
				return true;
			}
			// Removes k from saved values
			existsOnLine[i, k] = existsOnColumn[j, k] = existsInBlock[3 * (i / 3) + (j / 3), k] = false;
		}
	}

	return false;
}

/// <summary>
/// Gets the number of possible values in cell
/// </summary>
int GetPossibilities(int[,] grid, int i, int j)
{
	int count = 0;
	for (int k = 0; k < 9; k++) // For each value, checks if it is valid in the cell
		if (!existsOnLine[i, k] && !existsOnColumn[j, k] && !existsInBlock[3 * (i / 3) + (j / 3), k])
			count++;
	return count;
}

/// <summary>
/// Solves the specified grid
/// </summary>
bool Solve(int[,] grid)
{
	// Initializes global arrays to false
	for (int i = 0; i < 9; i++)
		for (int j = 0; j < 9; j++)
			existsOnLine[i, j] = existsOnColumn[i, j] = existsInBlock[i, j] = false;

	// Saves already present values in arrays
	int k;
	for (int i = 0; i < 9; i++)
		for (int j = 0; j < 9; j++)
			if ((k = grid[i, j]) != 0)
				existsOnLine[i, k - 1] = existsOnColumn[j, k - 1] = existsInBlock[3 * (i / 3) + (j / 3), k - 1] = true;

	// Creates the list of cells, sorted by number of possibilities
	positions = new List<EmptyCell>();

	for (int i = 0; i < 9; i++)
		for (int j = 0; j < 9; j++)
			if (grid[i, j] == 0)
				positions.Add(new EmptyCell() { Y = i, X = j, Possibilities = GetPossibilities(grid, i, j) });

	positions = positions.OrderBy(p => p.Possibilities).ToList();

	// Runs the backtracking algorithm from the start of the list
	bool ret = IsValid(grid, 0);

	return ret;
}

/// <summary>
/// Displays the grid in console
/// </summary>
void ShowGrid(int[,] grid)
{
	for (int i = 0; i < 9; i++)
	{
		for (int j = 0; j < 9; j++)
			Console.Write(grid[i, j]);
		Console.WriteLine();
	}
}

int[,] testGrid = new int[9, 9] {
	{ 9, 0, 0, 1, 0, 0, 0, 0, 5 },
	{ 0, 0, 5, 0, 9, 0, 2, 0, 1 },
	{ 8, 0, 0, 0, 4, 0, 0, 0, 0 },
	{ 0, 0, 0, 0, 8, 0, 0, 0, 0 },
	{ 0, 0, 0, 7, 0, 0, 0, 0, 0 },
	{ 0, 0, 0, 0, 2, 6, 0, 0, 9 },
	{ 2, 0, 0, 3, 0, 0, 0, 0, 6 },
	{ 0, 0, 0, 2, 0, 0, 9, 0, 0 },
	{ 0, 0, 1, 9, 0, 4, 5, 7, 0 }
};

Console.WriteLine("Base grid");
ShowGrid(testGrid);

Solve(testGrid);

Console.WriteLine("Solved grid");
ShowGrid(testGrid);