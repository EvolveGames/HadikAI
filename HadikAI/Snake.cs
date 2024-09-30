using System;
using System.Collections.Generic;

namespace HadikAI;
public class Snake
{
    public List<(int x, int y)> Body { get; private set; }
    public (int x, int y) Direction { get; set; }
    public (int x, int y) Food { get; private set; }
    private int width;
    private int height;
    private Random random;

    public Snake(int width, int height)
    {
        this.width = width;
        this.height = height;
        Body = new List<(int x, int y)> { (5, 5) }; // Počáteční pozice hada
        Direction = (0, 1); // Počáteční směr doprava
        random = new Random();
        PlaceFood();
    }

    public void PlaceFood()
    {
        while (true)
        {
            Food = (random.Next(0, width), random.Next(0, height));
            if (!Body.Contains(Food))
                break;
        }
    }

    public void Move()
    {
        var newHead = (Body[0].x + Direction.x, Body[0].y + Direction.y);

        // Kontrola kolizí s okraji


        if (newHead.Item1 < 0 || newHead.Item1 >= width || newHead.Item2 < 0 || newHead.Item2 >= height || Body.Contains(newHead))
        {
            throw new InvalidOperationException("Koliduje s okrajem nebo se sebou samým.");
        }

        Body.Insert(0, newHead);

        // Kontrola jídla
        if (newHead == Food)
        {
            PlaceFood(); // Nové jídlo
        }
        else
        {
            Body.RemoveAt(Body.Count - 1); // Odstranění posledního segmentu
        }
    }
}
