using Backtrack;

// Variables globales (tableaux) pour la mémorisation
bool[,] existsOnLine = new bool[9, 9];
bool[,] existsOnColumn = new bool[9, 9];
bool[,] existsInBlock = new bool[9, 9];
List<EmptyValue> positions = new List<EmptyValue>();

bool IsValid(int[,] grid, int position)
{
	if (position >= positions.Count || positions[position] == null)
		return true;

	int j = positions[position].X, i = positions[position].Y;

	for (int k = 0; k < 9; k++)
	{
		// Vérifie dans les tableaux si la valeur est déjà présente
		if (!existsOnLine[i, k] && !existsOnColumn[j, k] && !existsInBlock[3 * (i / 3) + (j / 3), k])
		{
			// Ajoute k aux valeurs enregistrées
			existsOnLine[i, k] = existsOnColumn[j, k] = existsInBlock[3 * (i / 3) + (j / 3), k] = true;

			if (IsValid(grid, position + 1))
			{
				// Ecrit le choix valide dans la grille
				grid[i, j] = k + 1;
				return true;
			}
			// Supprime k des valeurs enregistrées
			existsOnLine[i, k] = existsOnColumn[j, k] = existsInBlock[3 * (i / 3) + (j / 3), k] = false;
		}
	}

	return false;
}

// Calcule le nombre de valeurs possibles pour une case vide
int GetPossibilities(int[,] grid, int i, int j)
{
	int count = 0;
	for (int k = 0; k < 9; k++)
		if (!existsOnLine[i, k] && !existsOnColumn[j, k] && !existsInBlock[3 * (i / 3) + (j / 3), k])
			count++;
	return count;
}

bool Solve(int[,] grid)
{
	// Initialise les tableaux
	for (int i = 0; i < 9; i++)
		for (int j = 0; j < 9; j++)
			existsOnLine[i, j] = existsOnColumn[i, j] = existsInBlock[i, j] = false;

	// Enregistre dans les tableaux toutes les valeurs déjà présentes 
	int k;
	for (int i = 0; i < 9; i++)
		for (int j = 0; j < 9; j++)
			if ((k = grid[i, j]) != 0)
				existsOnLine[i, k - 1] = existsOnColumn[j, k - 1] = existsInBlock[3 * (i / 3) + (j / 3), k - 1] = true;

	// crée et remplit une liste pour les cases vides à visiter
	positions = new List<EmptyValue>();

	for (int i = 0; i < 9; i++)
		for (int j = 0; j < 9; j++)
			if (grid[i, j] == 0)
				positions.Add(new EmptyValue() { Y = i, X = j, Possibilities = GetPossibilities(grid, i, j) });

	positions = positions.OrderBy(p => p.Possibilities).ToList();

	bool ret = IsValid(grid, 0);

	return ret;
}

void ShowGrid(int[,] grid)
{
	for (int i = 0; i < 9; i++)
	{
		for (int j = 0; j < 9; j++)
			Console.Write(grid[i, j]);
		Console.WriteLine();
	}
}

int[,] grille = new int[9, 9] {
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

Console.WriteLine("Grille avant");
ShowGrid(grille);

Solve(grille);

Console.WriteLine("Grille apres");
ShowGrid(grille);