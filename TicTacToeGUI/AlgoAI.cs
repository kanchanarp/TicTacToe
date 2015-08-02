using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToeGUI
{
    public class AlgoAI
    {
          public int[,] grid = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
          public int[, ,] line = new int[8, 3, 2] { { { 0, 0 }, { 0, 1 }, { 0, 2 } }, { { 2, 0 }, { 2, 1 }, { 2, 2 } }, { { 0, 0 }, { 1, 0 }, { 2, 0 } }, { { 0, 2 }, { 1, 2 }, { 2, 2 } }, { { 1, 0 }, { 1, 1 }, { 1, 2 } }, { { 0, 1 }, { 1, 1 }, { 2, 1 } }, { { 0, 0 }, { 1, 1 }, { 2, 2 } }, { { 2, 0 }, { 1, 1 }, { 0, 2 } } };
          public int[,][] mapping = new int[3, 3][];
          public int[] lastMarked=new int[2];
          public Difficulty difficulyLevel;

          #region Game setup
          public void init(){
            mapping[0, 0] = new int[] { 0, 2, 6 };
            mapping[0, 1] = new int[] { 0, 5 };
            mapping[0, 2] = new int[] { 0, 3, 7 };
            mapping[1, 0] = new int[] { 2, 4 };
            mapping[1, 1] = new int[] { 4, 5, 6, 7 };
            mapping[1, 2] = new int[] { 3, 4 };
            mapping[2, 0] = new int[] { 1, 2, 7 };
            mapping[2, 1] = new int[] { 1, 5 };
            mapping[2, 2] = new int[] { 1, 3, 6 };
        }
        public void setDifficulty(Difficulty dif) {
            difficulyLevel = dif;
        }
        public void setBack() { 
         grid = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
         line = new int[8, 3, 2] { { { 0, 0 }, { 0, 1 }, { 0, 2 } }, { { 2, 0 }, { 2, 1 }, { 2, 2 } }, { { 0, 0 }, { 1, 0 }, { 2, 0 } }, { { 0, 2 }, { 1, 2 }, { 2, 2 } }, { { 1, 0 }, { 1, 1 }, { 1, 2 } }, { { 0, 1 }, { 1, 1 }, { 2, 1 } }, { { 0, 0 }, { 1, 1 }, { 2, 2 } }, { { 2, 0 }, { 1, 1 }, { 0, 2 } } };
         lastMarked=new int[2];
        }
          #endregion

          #region Game play
        public int getWin() {
            for (int i = 0; i < 8; i++)
            {
                if (grid[line[i, 0, 0], line[i, 0, 1]] + grid[line[i, 1, 0], line[i, 1, 1]] + grid[line[i, 2, 0], line[i, 2, 1]] == -3) {  return 1; }
                else if (grid[line[i, 0, 0], line[i, 0, 1]] + grid[line[i, 1, 0], line[i, 1, 1]] + grid[line[i, 2, 0], line[i, 2, 1]] == 3) {  return -1; }
                

            }
            return 0;
        }
        public void getInput(int u,int k,int j)
        {
            
            grid[k, j] = u;
            lastMarked[0] = k;
            lastMarked[1] = j;
        }

        public int[] mark(int u, int i, int j)
        {
            grid[i, j] = u;
            int[] ret = { i, j };
            lastMarked[0] = i;
            lastMarked[1] = j;
            return ret;
        }
        public string playOCont(int k, int j, int C)
        {

            bool state = false;
 
                if (C == 9) return ("Drawn");
                switch (C)
                {
                    case 1:
                        if (grid[1, 1] == 1) mark(-1, 0, 0);
                        else { mark(-1, 1, 1); }
                        
                        break;
                    case 3:
                        if ((grid[0, 0] == 1 && grid[2, 2] == 1 && grid[1, 1] == -1) || (grid[2, 0] == 1 && grid[0, 2] == 1 && grid[1, 1] == -1)) mark(-1, 0, 1);
                        else state = win(-1);

                        break;
                    default:
                        for (int i = 0; i < 8; i++)
                        {
                            if (grid[line[i, 0, 0], line[i, 0, 1]] + grid[line[i, 1, 0], line[i, 1, 1]] + grid[line[i, 2, 0], line[i, 2, 1]] == 3) return ("You win");
                        }
                        state = win(-1);
                        break;
                }

               if (state) return ("You lose");
            return "";
            
        }
        public string playXCont()
        {
            for (int i = 0; i < 8; i++)
            {
                if (grid[line[i, 0, 0], line[i, 0, 1]] + grid[line[i, 1, 0], line[i, 1, 1]] + grid[line[i, 2, 0], line[i, 2, 1]] == -3) return ("You win");
            }
            if (win(1))
            {
                return ("You lose");
            }
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (grid[i,j] == 0) {
                        return "";
                    }
                }
            }

            return "Drawn";
                
        }
          bool checkLine(int type, int i)
        {
            if (grid[line[i, 0, 0], line[i, 0, 1]] + grid[line[i, 1, 0], line[i, 1, 1]] + grid[line[i, 2, 0], line[i, 2, 1]] == type * 2) return (true);
            else return (false);
        }

          bool checkFork(int type, int i, int j)
        {
            int c = 0;
            grid[i, j] = type;
            foreach (int k in mapping[i, j])
            {
                if (checkLine(type, k)) c++;
            }
            grid[i, j] = 0;
            if (c > 1) return (true);
            return (false);
        }

          public string playX(int k, int j)
        {
            int C = 0;
            mark(1, 0, 0);
            C++;
            for (; ; )
            {
                getInput(-1,k,j);
                C++;
                for (int i = 0; i < 8; i++)
                {
                    if (grid[line[i, 0, 0], line[i, 0, 1]] + grid[line[i, 1, 0], line[i, 1, 1]] + grid[line[i, 2, 0], line[i, 2, 1]] == -3) return ("You win");
                }
                if (win(1)) return ("You lose");
                C++;
                if (C == 9) return ("Drawn");
            }
        }

          public string playO(int k, int j)
        {
            int C = 0;
            bool state = false;
            for (; ; )
            {
                getInput(1,k,j);
                C++;
                if (C == 9) return ("Drawn");
                switch (C)
                {
                    case 1:
                        if (grid[1, 1] == 1) mark(-1, 0, 0);
                        else mark(-1, 1, 1);
                        C++;
                        break;
                    case 3:
                        if ((grid[0, 0] == 1 && grid[2, 2] == 1 && grid[1, 1] == -1) || (grid[2, 0] == 1 && grid[0, 2] == 1 && grid[1, 1] == -1)) mark(-1, 0, 1);
                        else state = win(-1);
                        C++;
                        break;
                    default:
                        for (int i = 0; i < 8; i++)
                        {
                            if (grid[line[i, 0, 0], line[i, 0, 1]] + grid[line[i, 1, 0], line[i, 1, 1]] + grid[line[i, 2, 0], line[i, 2, 1]] == 3) return ("You win");
                        }
                        state = win(-1);
                        C++;
                        break;
                }
                if (state) return ("You lose");
            }
        }
        #endregion

          #region Algorithm
          public bool win(int u)
        {
            
            for (int i = 0; i < 8; i++)
            {
                if (checkLine(u, i))
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (grid[line[i, j, 0], line[i, j, 1]] == 0)
                        {
                            mark(u, line[i, j, 0], line[i, j, 1]);
                            break;
                        }
                    }

                    return (true);
                }
            }
            block(u);
            return (false);
        }

          void block(int u)
        {

            
                for (int i = 0; i < 8; i++)
                {
                    if (checkLine(-u, i))
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (grid[line[i, j, 0], line[i, j, 1]] == 0)
                            {
                                mark(u, line[i, j, 0], line[i, j, 1]);
                                break;
                            }
                        }
                        return;
                    }
                }

                if (difficulyLevel == Difficulty.HARD || difficulyLevel == Difficulty.MEDIUM)
                {
                    fork(u);
                }
                else { 
                    oppCorner(u); 
                }
        }

          void fork(int u)
        {
            
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (grid[i, j] == 0 && checkFork(u, i, j))
                        {
                            mark(u, i, j);

                            return;
                        }
                    }
                }
            
            if (difficulyLevel == Difficulty.HARD)
            {
                blockFork(u);
            }
            else {
                oppCorner(u);
            }
            
        }

          void blockFork(int u)
        {
            
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (grid[i, j] == 0 && checkFork(-u, i, j))
                        {
                            mark(u, i, j);
                            return;
                        }
                    }
                }
            
            oppCorner(u);
        }

          void oppCorner(int u)
        {
            if (grid[0, 0] == -u && grid[2, 2] == 0) {
                mark(u, 2, 2); 
            }
            else if (grid[0, 0] == 0 && grid[2, 2] == -u)
            {
                mark(u, 0, 0); 
            }
            else if (grid[0, 2] == 0 && grid[2, 0] == -u)
            {
                mark(u, 0, 2); 
            }
            else if (grid[0, 2] == -u && grid[2, 0] == 0)
            {
                mark(u, 2, 0); 
            }
            else centre(u);
        }

          void centre(int u)
        {
            if (grid[1, 1] == 0)
            {
                mark(u, 1, 1);
                return;
            }
            emptyCorner(u);
        }

          void emptyCorner(int u)
        {
            
                if (grid[0, 0] == 0)
                {
                    mark(u, 0, 0);
                }
                else if (grid[2, 2] == 0)
                {
                    mark(u, 2, 2);
                }
                else if (grid[0, 2] == 0)
                {
                    mark(u, 0, 2);
                }
                else if (grid[2, 0] == 0)
                {
                    mark(u, 2, 0);
                }
                else emptySide(u);
            
            
            
        }
          public int[] getlastMarked() {
              return lastMarked;
          }
          void emptySide(int u)
        {
             
                for (int i = 0; i < 4; i++)
                {
                    if (grid[line[i, 0, 0], line[i, 0, 1]] == 0 && grid[line[i, 1, 0], line[i, 1, 1]] == 0 && grid[line[i, 2, 0], line[i, 2, 1]] == 0)
                    {
                        mark(u, line[i, 1, 0], line[i, 1, 1]);
                        break;
                    }
                    //return;
                }

        }
          #endregion

    }
}
