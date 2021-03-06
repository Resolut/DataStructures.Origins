﻿using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2
{
    public class Vertex<T>
    {
        public bool Hit;
        public T Value;
        public Vertex(T val)
        {
            Value = val;
            Hit = false;
        }
    }

    public class SimpleGraph<T>
    {
        public Vertex<T>[] vertex;
        public int[,] m_adjacency;
        public int max_vertex;

        public SimpleGraph(int size)
        {
            max_vertex = size;
            m_adjacency = new int[size, size];
            vertex = new Vertex<T>[size];
        }

        public List<Vertex<T>> BreadthFirstSearch(int VFrom, int VTo)
        {
            List<Vertex<T>> path = new List<Vertex<T>>();           // путь из вершины VFrom в VTo
            List<Vertex<T>> adjVertex = new List<Vertex<T>>();      // список смежных вершин
            Queue<Vertex<T>> adjVQueue = new Queue<Vertex<T>>();    // очередь из смежных вершин
            foreach (var item in vertex) { item.Hit = false; }      // все вершины делаем непосещёнными

            Vertex<T> currentVertex = vertex[VFrom];    // текущая вершина в списке vertex
            currentVertex.Hit = true;
            path.Add(currentVertex); // добавление начальной вершины

            while (true)
            {
                adjVertex.Clear(); // сбрасываем список смежных вершин для текущего узла
                // формируем список непосещенных смежных вершин
                // с каждым проходом число смежных непосещенных узлов будет меньше
                adjVertex.AddRange(Array.FindAll(vertex, (item) =>
                    !item.Hit &&
                    item != currentVertex &&
                    IsEdge(Array.IndexOf(vertex, currentVertex), Array.IndexOf(vertex, item))));

                if (adjVertex.Count == 0)
                {
                    if (adjVQueue.Count == 0)
                    {
                        path.Clear();
                        return path;
                    }

                    currentVertex = adjVQueue.Dequeue();
                    path.Add(currentVertex);
                }
                else
                {
                    if (adjVertex[0] == vertex[VTo])
                    {
                        path.Add(adjVertex[0]);
                        Vertex<T> lastVertex = path[path.Count - 1]; // последняя вершина в пути
                        List<Vertex<T>> tempList = new List<Vertex<T>> { lastVertex }; // кратчайший путь VTo -> VFrom

                        // определяем кратчайший путь, фильтруя вершины из списка path
                        int currentIndex = path.Count - 1; // индекс вершины
                        int startIndex = 0;
                        while (currentIndex > startIndex)
                        {
                            if (IsEdge(Array.IndexOf(vertex, path[currentIndex]), Array.IndexOf(vertex, path[startIndex])))
                            {
                                tempList.Add(path[startIndex]);
                                currentIndex = startIndex;
                                startIndex = 0;
                            }
                            else
                                ++startIndex;
                        }

                        tempList.Reverse(); // VFrom -> VTo
                        path = tempList;

                        return path;
                    }

                    adjVertex[0].Hit = true;
                    adjVQueue.Enqueue(adjVertex[0]);
                }
            }
        }

        public List<Vertex<T>> DepthFirstSearch(int VFrom, int VTo)
        {
            List<Vertex<T>> path = new List<Vertex<T>>();           // путь из вершины VFrom в VTo
            List<Vertex<T>> adjVertex = new List<Vertex<T>>();      // список смежных вершин
            Stack<Vertex<T>> localStack = new Stack<Vertex<T>>();   // очищаем стек
            foreach (var item in vertex) { item.Hit = false; }      // все вершины делаем непосещенными

            Vertex<T> currentVertex = vertex[VFrom];    // текущая вершина в списке vertex
            currentVertex.Hit = true;                   // делаем вершину посещенной
            localStack.Push(currentVertex);             // добавляем текущую вершину в стек
                                                        // ищем в смежных вершинах VFrom вершину VTo
            while (true)
            {
                if (IsEdge(Array.IndexOf(vertex, currentVertex), VTo))
                {
                    localStack.Push(vertex[VTo]);
                    path.AddRange(localStack);
                    path.Reverse(); // путь из начальной вершины в конечную
                    return path;
                }
                else
                {
                    // с каждым проходом число смежных непосещенных узлов будет меньше
                    adjVertex = new List<Vertex<T>>(); // сбрасываем список смежных вершин для текущего узла
                    adjVertex.AddRange(Array.FindAll(vertex, (item) =>
                        !item.Hit &&
                        item != currentVertex &&
                        IsEdge(Array.IndexOf(vertex, currentVertex), Array.IndexOf(vertex, item))));

                    if (adjVertex.Count == 0)
                    {
                        localStack.Pop();
                        if (localStack.Count == 0) return path;
                        else
                        {
                            currentVertex = localStack.Peek();
                            currentVertex.Hit = true;
                        }
                    }
                    else
                    {
                        currentVertex = adjVertex[0];
                        currentVertex.Hit = true;
                        localStack.Push(currentVertex);
                    }
                }
            }
        }

        public List<Vertex<T>> WeakVertices()
        {
            List<Vertex<T>> WeakVertList = new List<Vertex<T>>();   // список вершин, не входящих в треугольники
            List<Vertex<T>> adjVertex = new List<Vertex<T>>();      // сбрасываем список смежных вершин для текущего узла

            foreach (var currentVert in vertex)
            {
                adjVertex.Clear();
                adjVertex.AddRange(Array.FindAll(vertex, (item) =>
                item != currentVert &&
                IsEdge(Array.IndexOf(vertex, currentVert), Array.IndexOf(vertex, item))));

                if (adjVertex.Count < 2) WeakVertList.Add(currentVert);
                else
                {
                    bool isFreeVert = true; // флаг вершины, не входящей в треугольник

                    for (int i = 0; i < adjVertex.Count; i++)
                    {
                        for (int j = i + 1; j < adjVertex.Count; j++)
                        {
                            if (IsEdge(Array.IndexOf(vertex, adjVertex[i]), Array.IndexOf(vertex, adjVertex[j])))
                            {
                                isFreeVert = false;
                                break;
                            }
                        }
                    }

                    if (isFreeVert) WeakVertList.Add(currentVert);
                }
            }

            return WeakVertList;
        }

        public void AddVertex(T value)
        {
            // ваш код добавления новой вершины 
            // с значением value 
            // в свободную позицию массива vertex
            int index = Array.IndexOf(vertex, null);
            if (index != -1)
                vertex[index] = new Vertex<T>(value);
        }

        public void RemoveVertex(int v)
        {
            // ваш код удаления вершины со всеми её рёбрами
            if (v >= 0 && v < max_vertex && vertex[v] != null)
            {
                vertex[v] = null; // удаление вершины
                int rows = vertex.GetUpperBound(0) + 1;
                for (int i = 0; i < rows; i++)
                {
                    m_adjacency[v, i] = 0; // удаление рёбер
                    m_adjacency[i, v] = 0;
                }
            }
        }

        public bool IsEdge(int v1, int v2)
        {
            // true если есть ребро между вершинами v1 и v2
            if (vertex[v1] != null && vertex[v2] != null)
                if (m_adjacency[v1, v2] == 1 && m_adjacency[v2, v1] == 1) return true;

            return false;
        }

        public void AddEdge(int v1, int v2)
        {
            // добавление ребра между вершинами v1 и v2
            if (!IsEdge(v1, v2) && vertex[v1] != null && vertex[v2] != null)
            {
                if ((v1 >= 0 && v1 < max_vertex) &&
                    (v2 >= 0 && v2 < max_vertex))
                {
                    m_adjacency[v1, v2] = 1;
                    m_adjacency[v2, v1] = 1;
                }
            }
        }

        public void RemoveEdge(int v1, int v2)
        {
            // удаление ребра между вершинами v1 и v2
            if (IsEdge(v1, v2))
            {
                if ((v1 >= 0 && v1 < max_vertex) &&
                    (v2 >= 0 && v2 < max_vertex))
                {
                    m_adjacency[v1, v2] = 0;
                    m_adjacency[v2, v1] = 0;
                }
            }
        }
    }

}
