using System.Diagnostics;

namespace GraphLab_1
{
	public class Program
	{
		public const int V = 500;

		private static Random _random { get; set; } = new Random();

		public static int MinDistance(int[] dist, bool[] sptSet)
		{
			int min = int.MaxValue;
			int min_index = -1;
			for (int i = 0; i < V; i++)
			{
				if (sptSet[i] == false && dist[i] <= min)
				{
					min = dist[i];
					min_index = i;
				}
			}

			return min_index;
		}

		public static void Find_N(int[] n, int[,] graph)
		{
			for (int u = 0; u < V; u++)
			{
				n[u] = 0;
			}

			for (int u = 0; u < V; u++)
			{
				for (int v = 0; v < V; v++)
				{
					if (graph[u, v] != 0)
					{
						n[v]++;
					}
				}
			}
		}

		public static void FindRang(int[] rang, int[,] graph)
		{
			for (int u = 0; u < V; u++)
			{
				rang[u] = 0;
			}

			bool flag = true;
			while (flag)
			{
				flag = true;
				for (int u = 0; u < V; u++)
				{
					for (int v = 0; v < V; v++)
					{
						if (graph[u, v] != 0 && rang[u] + 1 > rang[v])
						{
							rang[v] = rang[u] + 1;
							flag = false;
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
		}

		public static void FindV(int[] v, int[] rang)
		{
			int num = V;
			int pos = 0;
			int cur_rang = 0;
			while (num != 0)
			{
				for (int i = 0; i < V; i++)
				{
					if (rang[i] == cur_rang)
					{
						v[pos] = i;
						pos++;
						num--;
					}
				}
				cur_rang++;
			}
		}

		public static double Fast(int[,] graph, int src)
		{
			var dis = new int[V];
			var vis = new bool[V];

			for (int i = 0; i < V; i++)
			{
				dis[i] = int.MaxValue;
				vis[i] = false;
			}

			dis[src] = 0;
			vis[src] = true;

			int head = 0;
			int tail = 1;

			var sw = new Stopwatch();

			sw.Start();

			var q = new int[V * 2];
			q[tail] = src;
			while (head < tail)
			{
				head += 1;
				int v = q[head];
				vis[v] = false;

				for (int i = 0; i < V; i++)
				{
					if (dis[i] > dis[v] + graph[v, i] && graph[v, i] != 0 && dis[v] != int.MaxValue)
					{
						dis[i] = dis[v] + graph[v, i];
						if (vis[i] == false)
						{
							tail += 1;
							q[tail] = i;
							vis[i] = true;
						}
					}
				}
			}

			sw.Stop();

			return sw.ElapsedMilliseconds;
		}

		public static double BellmanFord(int[,] graph, int src)
		{
			var dist = new int[V];

			for (int i = 0; i < V; i++)
			{
				dist[i] = int.MaxValue;
			}

			dist[src] = 0;

			var sw = new Stopwatch();

			sw.Start();

			for (int u = 0; u < V; u++)
			{
				for (int v = 0; v < V; v++)
				{
					if (graph[u, v] != 0 && dist[u] < int.MaxValue && dist[u] + graph[u, v] < dist[v])
					{
						dist[v] = dist[u] + graph[u, v];
					}
				}
			}

			sw.Stop();

			return sw.ElapsedMilliseconds;
		}

		public static double Dijkstra(int[,] graph, int src)
		{
			var dist = new int[V];  // создали массив для хранения самого короткого пути
			var sptSet = new bool[V]; // флаг, включена ли вершина в дерево кратчайшего пути

			for (int i = 0; i < V; i++)
			{
				dist[i] = int.MaxValue; //присваиваем всем вершинам в массиве максимальное значение(бесконечность)
				sptSet[i] = false; //флаг указывает что все вершины не включены в кратчайший путь
			}

			dist[src] = 0; //расстояние от источника

			var sw = new Stopwatch();

			sw.Start();

			for (int count = 0; count < V; count++)
			{
				// выбираем вершину минимального расстояния из набора
				int u = MinDistance(dist, sptSet); //нам вернули индекс вершины минимального расстояния
				sptSet[u] = true; //вершина по этому индексу теперь входит в кратчайший путь
				for (int v = 0; v < V; v++)
				{
					if (!sptSet[v] && graph[u, v] != 0 //обновляем dist[v], только если: его нет в sptSet && есть ребро от U до V
						&& dist[u] != int.MaxValue //+ расстояние от источника до начала ребра не бесконечно
						&& dist[u] + graph[u, v] < dist[v]) //+ расстояние до V проходящее через U меньше чем текущее значение расстояния до V
						dist[v] = dist[u] + graph[u, v]; //присваиваем новое значение dist[v]
				}
			}

			sw.Stop();

			return sw.ElapsedMilliseconds;
		}

		public static void Main(string[] args)
		{
			var graph = new int[V, V];

			InitializeGraph(graph);

			var time_F = Fast(graph, 0);
			var time_D = Dijkstra(graph, 0);
			var time_B = BellmanFord(graph, 0);

			Console.WriteLine($"Time Fast algorithm: {time_F} ms");
			Console.WriteLine($"Time Dijstra's algorithm: {time_D} ms");
			Console.WriteLine($"Time Bellman-Ford's algorithm: {time_B} ms");
		}

		private static void InitializeGraph(int[,] graph)
		{
			for (int i = 0; i < V; i++)
			{
				int a = _random.Next(1, 11);

				for (int j = 0; j < V; j++)
				{
					if (j % a == 0 && j != 0 && i != j)
						graph[i, j] = _random.Next(3, 15);
					else
						graph[i, j] = 0;
				}
			}
		}
	}
}