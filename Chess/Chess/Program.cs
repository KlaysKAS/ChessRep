using System;
using System.Text;

namespace Chess
{

    class board
    {
        public string[,] b;
        
        public board()
        {
            b = new string[8, 8]
               {{"BR", "BH", "BB", "BQ", "BK", "BB", "BH", "BR"},
                {"BP", "BP", "BP", "BP", "BP", "BP", "BP", "BP"},
                {null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null},
                {"WP", "WP", "WP", "WP", "WP", "WP", "WP", "WP"},
                {"WR", "WH", "WB", "WK", "WQ", "WB", "WH", "WR"}};
        }
        
        public void print()
        {
            Console.WriteLine("  a  b  c  d  e  f  g  h");
            for (int i = 0; i < 8; ++i)
            {
                Console.WriteLine(" -------------------------");
                Console.Write(8 - i);
                for (int j = 0; j < 8; ++j)
                {
                    if (b[i, j] != null)
                        Console.Write('|' + b[i, j]);
                    else
                        Console.Write("|  ");
                }
                Console.WriteLine("|" + (8 - i));
            }
            Console.WriteLine(" -------------------------");
            Console.WriteLine("  a  b  c  d  e  f  g  h");
        }
    }

    abstract class Figure
    {
        public abstract bool check(int x, int y, ref board b);
        public abstract int move(int x, int y, ref board b);
        public abstract bool isIt(int x, int y);
        
    }

    class pawn : Figure
    {
        private bool team;
        private int x, y;
        private bool moved;

        public bool isAlive;

        public pawn(int x, int y, bool team)
        {
            this.x = x;
            this.y = y;
            this.team = team;
            this.moved = false;
            this.isAlive = true;
        }

        public override bool isIt(int x, int y)
        {
            return this.x == x && this.y == y;
        }

        public override bool check(int x, int y, ref board b)
        {
            bool isMove, isTwo = false;
            if (team)
            {
                isMove = x == this.x && y < this.y;
                if (!moved)
                {
                    isMove &= this.y - y <= 2;
                    isTwo = this.y - y == 2 ? true : false;
                } else
                {
                    isMove &= this.y - y == 1;
                }
            } else
            {
                isMove = x == this.x && y > this.y;
                if (!moved && isMove)
                {
                    isMove &= y - this.y <= 2;
                    isTwo = y - this.y == 2 ? true : false;
                }
                else
                {
                    isMove &= y - this.y == 1;
                }
            }

            

            bool isEat;
            if (team)
            {
                isEat = this.y - y == 1 && Math.Abs(this.x - x) == 1 && b.b[y, x] != null && b.b[y, x][0] == 'B';
            } else
            {
                isEat = y - this.y == 1 && Math.Abs(this.x - x) == 1 && b.b[y, x] != null && b.b[y, x][0] == 'W';
            }

            bool isSpace = false;
            if (!isEat && isMove)
            {
                isSpace = b.b[y, x] == null;
                if (isTwo && isSpace)
                {
                    if (team)
                    {
                        isSpace &= b.b[this.y - 1, x] == null;
                    } else
                    {
                        isSpace &= b.b[this.y + 1, x] == null;
                    }
                }   
            }

            return isEat || isMove && isSpace;
        }

        public override int move(int x, int y, ref board b)
        {
            if (check(x, y, ref b))
            {
                b.b[y, x] = b.b[this.y, this.x];
                b.b[this.y, this.x] = null;
                this.y = y;
                this.x = x;
                moved = true;
                Console.Clear();
                b.print();
                return 0;
            } else
            {
                Console.WriteLine("Ход не возможен");
                return 1;
            }
            
        }

    }

    class rook : Figure
    {
        private bool team;
        private int x, y;

        public bool isAlive;

        public rook(int x, int y, bool team)
        {
            this.x = x;
            this.y = y;
            this.team = team;
            this.isAlive = true;
        }

        public override bool isIt(int x, int y)
        {
            return this.x == x && this.y == y;
        }

        public override bool check(int x, int y, ref board b)
        {
            bool isMove = true;
            {
                if (this.x == x && this.y != y)
                {
                    int t = this.y;
                    t += this.y > y ? -1 : 1;
                    while (t != y)
                    {
                        if (b.b[t, x] != null)
                        {
                            isMove = false;
                            break;
                        }
                        t += this.y > y ? -1 : 1;
                    }
                    if (t == y)
                    {
                        isMove = b.b[t, x] == null;
                        if (!isMove)
                        {
                            if (team)
                                isMove |= b.b[t, x][0] == 'B';
                            else
                                isMove |= b.b[t, x][0] == 'W';
                        }
                    }
                }
                else if (this.x != x && this.y == y)
                {
                    int t = this.x;
                    t += this.x > x ? -1 : 1;
                    while (t != x)
                    {
                        if (b.b[y, t] != null)
                        {
                            isMove = false;
                            break;
                        }
                        t += this.x > x ? -1 : 1;
                    }
                    if (t == x)
                    {
                        isMove = b.b[y, t] == null;
                        if (!isMove)
                        {
                            isMove = team ? b.b[t, x][0] == 'B' : b.b[t, x][0] == 'W';
                        }
                    }
                }
                else
                    isMove = false;
            }

            return isMove;
        }

        public override int move(int x, int y, ref board b)
        {
            if (check(x, y, ref b))
            {
                b.b[y, x] = b.b[this.y, this.x];
                b.b[this.y, this.x] = null;
                this.y = y;
                this.x = x;
                Console.Clear();
                b.print();
                return 0;
            }
            else
            {
                Console.WriteLine("Ход не возможен");
                return 1;
            }
        }
    }

    class bishop : Figure
    {
        private bool team;
        private int x, y;

        public bool isAlive;

        public bishop(int x, int y, bool team)
        {
            this.x = x;
            this.y = y;
            this.team = team;
            this.isAlive = true;
        }

        public override bool isIt(int x, int y)
        {
            return this.x == x && this.y == y;
        }

        public override bool check(int x, int y, ref board b)
        {
            bool isMove = true;
            {
                if (Math.Abs(this.x - x) == Math.Abs(this.y - y))
                {
                    int dx = this.x; dx += this.x > x ? -1 : 1;
                    int dy = this.y; dy += this.y > y ? -1 : 1;
                    while (dx != x)
                    {
                        if (b.b[dy, dx] != null)
                        {
                            isMove = false;
                            break;
                        }
                        dx += this.x > x ? -1 : 1;
                        dy += this.y > y ? -1 : 1;
                    }
                    if (dx == x)
                    {
                        if (b.b[dy, dx] != null)
                        {
                            isMove = false;
                            if (!isMove)
                            {
                                isMove = team ? b.b[dy, dx][0] == 'B' : b.b[dy, dx][0] == 'W';
                            }
                        }
                    }
                }
                else
                    isMove = false;
                    
            }

            return isMove;
        }

        public override int move(int x, int y, ref board b)
        {
            if (check(x, y, ref b))
            {
                b.b[y, x] = b.b[this.y, this.x];
                b.b[this.y, this.x] = null;
                this.y = y;
                this.x = x;
                Console.Clear();
                b.print();
                return 0;
            }
            else
            {
                Console.WriteLine("Ход не возможен");
                return 1;
            }
        }
    }

    class horse : Figure
    {
        private bool team;
        private int x, y;

        public bool isAlive;

        public horse(int x, int y, bool team)
        {
            this.x = x;
            this.y = y;
            this.team = team;
            this.isAlive = true;
        }

        public override bool isIt(int x, int y)
        {
            return this.x == x && this.y == y;
        }

        public override bool check(int x, int y, ref board b)
        {
            bool isMove;
            {
                if (Math.Abs(this.x - x) * Math.Abs(this.y - y) == 2)
                {
                    if (b.b[y, x] == null)
                        isMove = true;
                    else
                        isMove = team ? b.b[y, x][0] == 'B' : b.b[y, x][0] == 'W';
                }
                else
                    isMove = false;

            }

            return isMove;
        }

        public override int move(int x, int y, ref board b)
        {
            if (check(x, y, ref b))
            {
                b.b[y, x] = b.b[this.y, this.x];
                b.b[this.y, this.x] = null;
                this.y = y;
                this.x = x;
                Console.Clear();
                b.print();
                return 0;
            }
            else
            {
                Console.WriteLine("Ход не возможен");
                return 1;
            }
        }
    }

    class king : Figure
    {
        private bool team;
        private int x, y;

        public bool isAlive;

        public king(int x, int y, bool team)
        {
            this.x = x;
            this.y = y;
            this.team = team;
            this.isAlive = true;
        }

        public override bool isIt(int x, int y)
        {
            return this.x == x && this.y == y;
        }

        public override bool check(int x, int y, ref board b)
        {
            bool isMove;
            {
                if ((Math.Abs(this.x - x) == 1 || Math.Abs(this.y - y) == 1) && Math.Abs(this.x - x) + Math.Abs(this.y - y) != 0)
                {
                    if (b.b[y, x] == null)
                        isMove = true;
                    else
                        isMove = team ? b.b[y, x][0] == 'B' : b.b[y, x][0] == 'W';
                }
                else
                    isMove = false;

            }

            return isMove;
        }

        public override int move(int x, int y, ref board b)
        {
            if (check(x, y, ref b))
            {
                b.b[y, x] = b.b[this.y, this.x];
                b.b[this.y, this.x] = null;
                this.y = y;
                this.x = x;
                Console.Clear();
                b.print();
                return 0;
            }
            else
            {
                Console.WriteLine("Ход не возможен");
                return 1;
            }
        }
    }

    class queen : Figure
    {
        private bool team;
        private int x, y;

        public bool isAlive;

        public queen(int x, int y, bool team)
        {
            this.x = x;
            this.y = y;
            this.team = team;
            this.isAlive = true;
        }

        public override bool isIt(int x, int y)
        {
            return this.x == x && this.y == y;
        }


        public override bool check(int x, int y, ref board b)
        {
            bool isMove = true;
            {
                bool isH = this.x != x && this.y == y;
                bool isV = this.x == x && this.y != y;
                bool isX = Math.Abs(this.x - x) == Math.Abs(this.y - y);
                if (isH || isX || isV)
                {
                    int dx = this.x; 
                    if (isH || isX)
                        dx += this.x > x ? -1 : 1;
                    int dy = this.y;
                    if (isV || isX)
                        dy += this.y > y ? -1 : 1;
                    
                    while (dx != x || dy != y)
                    {
                        if (b.b[dy, dx] != null)
                        {
                            isMove = false;
                            break;
                        }
                        if (isH || isX)
                            dx += this.x > x ? -1 : 1;
                        if (isV || isX)
                            dy += this.y > y ? -1 : 1;
                    }
                    if (dx == x && dy == y)
                    {
                        if (b.b[dy, dx] != null)
                        {
                            isMove = false;
                            if (!isMove)
                            {
                                isMove = team ? b.b[dy, dx][0] == 'B' : b.b[dy, dx][0] == 'W';
                            }
                        }
                    }
                }
                else
                    isMove = false;

            }

            return isMove;
        }

        public override int move(int x, int y, ref board b)
        {
            if (check(x, y, ref b))
            {
                b.b[y, x] = b.b[this.y, this.x];
                b.b[this.y, this.x] = null;
                this.y = y;
                this.x = x;
                Console.Clear();
                b.print();
                return 0;
            }
            else
            {
                Console.WriteLine("Ход не возможен");
                return 1;
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            board b = new board();
            b.print();
            Console.WriteLine("Белые ходят первые");

            pawn[,] pawns = new pawn[2, 8];
            for (int i = 0; i < 8; ++i)
            {
                pawns[0, i] = new pawn(i, 6, true);
                pawns[1, i] = new pawn(i, 1, false);
            }

            rook[,] rooks = new rook[2, 2];
            for (int i = 0; i <8; i += 7)
            {
                rooks[0, i % 2] = new rook(i, 7, true);
                rooks[1, i % 2] = new rook(i, 0, false);
            }

            horse[,] horses = new horse[2, 2];
            horses[0, 0] = new horse(1, 7, true);
            horses[0, 1] = new horse(6, 7, true);
            horses[1, 0] = new horse(1, 0, false);
            horses[1, 1] = new horse(6, 0, false);

            bishop[,] bishops = new bishop[2, 2];
            bishops[0, 0] = new bishop(2, 7, true);
            bishops[0, 1] = new bishop(5, 7, true);
            bishops[1, 0] = new bishop(2, 0, false);
            bishops[1, 1] = new bishop(5, 0, false);

            queen[,] queens = new queen[2, 1];
            queens[0, 0] = new queen(4, 7, true);
            queens[1, 0] = new queen(3, 0, false);

            king[,] kings = new king[2, 1];
            kings[0, 0] = new king(3, 7, true);
            kings[1, 0] = new king(4, 0, false);

            bool turn = false;
            string move = Console.ReadLine();
            move = move.ToLower();
            while (move != "exit")
            {
                string[] splittedMove = move.Split('-');
                if (splittedMove[0] == "" || splittedMove.Length != 2)
                {
                    Console.WriteLine("Ход не введён или введён не верно, используйте формат \"e2-e4\"");
                } else
                {
                    bool check = splittedMove[0].Length == 2 && splittedMove[1].Length == 2;
                    check &= splittedMove[0][0] >= 'a' && splittedMove[0][0] <= 'h' && splittedMove[0][1] >= '1' && splittedMove[0][1] <= '8';
                    check &= splittedMove[1][0] >= 'a' && splittedMove[1][0] <= 'h'&& splittedMove[1][1] >= '1' && splittedMove[1][1] <= '8';
                    if (check)
                    {
                        int startX, startY, endX, endY;
                        startX = splittedMove[0][0] - 'a';
                        startY = '8' - splittedMove[0][1];
                        endX = splittedMove[1][0] - 'a';
                        endY = '8' - splittedMove[1][1];
                        
                        if (b.b[startY, startX] == null)
                        {
                            Console.WriteLine("Начальная позиция пуста");
                        } else
                        {      
                            if (b.b[startY, startX][0] == 'W' ^ turn)
                            {
                                
                                switch (b.b[startY, startX])
                                {
                                    case "WP":
                                        {
                                            for (int i = 0; i < 8; ++i)
                                            {
                                                if (pawns[0, i].isIt(startX, startY) && pawns[0, i].isAlive)
                                                {
                                                    int r = pawns[0, i].move(endX, endY, ref b);
                                                    if (r == 0)
                                                        turn = !turn;
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    case "BP":
                                        {
                                            for (int i = 0; i < 8; ++i)
                                            {
                                                if (pawns[1, i].isIt(startX, startY) && pawns[1, i].isAlive)
                                                {
                                                    int r = pawns[1, i].move(endX, endY, ref b);
                                                    if (r == 0)
                                                        turn = !turn;
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    case "WR":
                                        {
                                            for (int i = 0; i < 2; ++i)
                                            {
                                                if (rooks[0, i].isIt(startX, startY) && rooks[0, i].isAlive)
                                                {
                                                    int r = rooks[0, i].move(endX, endY, ref b);
                                                    if (r == 0)
                                                        turn = !turn;
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    case "BR":
                                        {
                                            for (int i = 0; i < 2; ++i)
                                            {
                                                if (rooks[1, i].isIt(startX, startY) && rooks[1, i].isAlive)
                                                {
                                                    int r = rooks[1, i].move(endX, endY, ref b);
                                                    if (r == 0)
                                                        turn = !turn;
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    case "WH":
                                        {
                                            for (int i = 0; i < 2; ++i)
                                            {
                                                if (horses[0, i].isIt(startX, startY) && horses[0, i].isAlive)
                                                {
                                                    int r = horses[0, i].move(endX, endY, ref b);
                                                    if (r == 0)
                                                        turn = !turn;
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    case "BH":
                                        {
                                            for (int i = 0; i < 2; ++i)
                                            {
                                                if (horses[1, i].isIt(startX, startY) && horses[1, i].isAlive)
                                                {
                                                    int r = horses[1, i].move(endX, endY, ref b);
                                                    if (r == 0)
                                                        turn = !turn;
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    case "WB":
                                        {
                                            for (int i = 0; i < 2; ++i)
                                            {
                                                if (bishops[0, i].isIt(startX, startY) && bishops[0, i].isAlive)
                                                {
                                                    int r = bishops[0, i].move(endX, endY, ref b);
                                                    if (r == 0)
                                                        turn = !turn;
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    case "BB":
                                        {
                                            for (int i = 0; i < 2; ++i)
                                            {
                                                if (bishops[1, i].isIt(startX, startY) && bishops[1, i].isAlive)
                                                {
                                                    int r = bishops[1, i].move(endX, endY, ref b);
                                                    if (r == 0)
                                                        turn = !turn;
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    case "WK":
                                        {
                                            if (kings[0, 0].isIt(startX, startY) && kings[0, 0].isAlive)
                                            {
                                                int r = kings[0, 0].move(endX, endY, ref b);
                                                if (r == 0)
                                                    turn = !turn;
                                            }
                                            break;
                                        }
                                    case "BK":
                                        {
                                            if (kings[1, 0].isIt(startX, startY) && kings[1, 0].isAlive)
                                            {
                                                int r = kings[1, 0].move(endX, endY, ref b);
                                                if (r == 0)
                                                    turn = !turn;
                                            }
                                            break;
                                        }
                                    case "WQ":
                                        {
                                            if (queens[0, 0].isIt(startX, startY) && queens[0, 0].isAlive)
                                            {
                                                int r = queens[0, 0].move(endX, endY, ref b);
                                                if (r == 0)
                                                    turn = !turn;
                                            }
                                            break;
                                        }
                                    case "BQ":
                                        {
                                            if (queens[1, 0].isIt(startX, startY) && queens[1, 0].isAlive)
                                            {
                                                int r = queens[1, 0].move(endX, endY, ref b);
                                                if (r == 0)
                                                    turn = !turn;
                                            }
                                            break;
                                        }
                                }
                            } else
                            {
                                Console.WriteLine("Сейчас ход противоположного игрока");
                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine("Ход введён не верно, используйте формат \"e2-e4\"");
                    }
                }
                move = Console.ReadLine();
                move = move.ToLower();
            }
            Console.WriteLine("Игра окончена. Нажмите любую клавишу для выхода");

            Console.ReadKey();
        }
    }
}
