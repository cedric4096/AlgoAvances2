namespace Dijkstra
{
    public class Parcours
    {
        
        static int V = 9; //taille 

        // trouve le chemin le moins long
        int minDistance(int[] dist, bool[] sptSet)
        {
            // On met au max 
            int min = int.MaxValue, min_index = -1;

            for (int v = 0; v < V; v++)
                if (sptSet[v] == false && dist[v] <= min)
                {
                    min = dist[v];
                    min_index = v;
                }

            return min_index;
        }

        // affichage
        private void printSolution(int[] dist, int n)
        {
            Console.Write("Point        Distance de la source\n");
            for (int i = 0; i < V; i++)
                Console.Write(i + " \t\t " + dist[i] + "\n");
        }

        // Algorithme de dijkstra
        public void dijkstra(int[,] graph, int src)
        {
            int[] dist = new int[V]; // distance stocké

            //  True si appartient au chemin le plus cours
            bool[] sptSet = new bool[V];

            // On met à l'infini et à false
            for (int i = 0; i < V; i++)
            {
                dist[i] = int.MaxValue;
                sptSet[i] = false;
            }

            // La distance du début au début est toujours 0 
            dist[src] = 0;

            // Trouver le chemin le plus court
            for (int count = 0; count < V - 1; count++)
            {
                // on prend le poids min
                int u = minDistance(dist, sptSet);

                //visité
                sptSet[u] = true;

                // On met à jour avec les valeurs suivante
                for (int v = 0; v < V; v++)

                    // On met à jour seulement si  : pas dans sptSet, liens entre u et v, et si le poids est inférieur à la valeur actuel de dist[v]
                    if (!sptSet[v] && graph[u, v] != 0 &&
                         dist[u] != int.MaxValue && dist[u] + graph[u, v] < dist[v])
                        dist[v] = dist[u] + graph[u, v];
            }

            printSolution(dist, V); // on affiche 
        }


    }
}
