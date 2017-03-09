#include <stdio.h>
#include <Windows.h>
#include <conio.h>
#include <stdbool.h>

#define KEY_UP 72
#define KEY_DOWN 80
#define KEY_LEFT 75
#define KEY_RIGHT 77


const int width = 100;
const int height = 25;

char table[100][25];	//window table
char usr = 'X';			//init user
int x = 2, y = 24;		// user x, y

int enemyX = 1, enemyX2 = 40, enemyY = 21, enemyY2 = 20, k = 1, k2 = 1; //enemy x, y and speed = 1
int enemyX3 = 50, enemyX4 = 98, enemyY3 = 17, enemyY4 = 16, k3 = 1, k4 = 1; 
int enemyX5 = 20, enemyX6 = 48, enemyY5 = 13, enemyY6 = 12, k5 = 1, k6 = 1;
int enemyX7 = 90, enemyX8 = 48, enemyY7 = 9, enemyY8 = 8, k7 = 1, k8 = 1;
int enemyX9 = 10, enemyX10 = 40, enemyY9 = 5, enemyY10 = 4, k9 = 1, k10 = 1;

char fruit = '@';
int fX = 15, fY = 20;
int f2X = 94, f2Y = 21;
int f3X = 65, f3Y = 16;
int f4X = 35, f4Y = 13;
int f5X = 75, f5Y = 9;
int f6X = 33, f6Y = 5;
int f7X = 12, f7Y = 1;
int f8X = 22, f8Y = 4;
int f9X = 25, f9Y = 8;
int f10X = 92, f10Y = 17;

int hitFruit = 0; //fruit hits
bool fHits[10];	

int score = 0;
bool die = false;
bool gameOver;

//prototypes
void setup();
void window();
void move();
void moveEnemy();
void massage();
void massageQuit();
bool inspectionMoving();

int main(void)
{
	//paint console
	system("color 0A");
	setup();
	
	
	while (!gameOver)
	{
		//game window
		window();
		//move enemy
		moveEnemy();
		//move user
		move();
		
		Sleep(50);
	}
	if (gameOver && die)
		massage();
	else
		massageQuit();

	return 0;
}

void setup()
{
	gameOver = false;
	printf("\n\tRUNNER_2000\n");
	printf("\n Your char - X");
	printf("\n hit all fruits - @\n dodging all enemies - & \n");
	printf("\n Move: up/down/Left/right arrows..");
	printf("\n To start press any key..");
	_getch();
}
//initialize game window
void window()
{
	int *pts = &score;
	 //init score
	if (hitFruit > 0) //score++ if usr hit fruit
	{
		*pts += 10;
		hitFruit = 0;
	}
	//update window
	system("cls");
	//top line with scores & quit info
	printf("  Score: %03i", *pts);
	printf("\t\t\t\t\t\t\t\t\tQuit: press 'q'\n");
	for (int i = 0; i < width; i++)
	{
		printf("#");
	}
	printf("\n");

	//middle
	for (int i = 1; i < height; i++)
	{
		for (int j = 0; j < width; j++)
		{
			//prints textures
			if (j == 0)
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			}
			else if (j == (width - 1))
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			}
			else if (((j > 1 && j < 25 && i == 23) || (j > 1 && j < 25 && i == 22)) || ((j > 26 && j < 50 && i == 23) || (j > 26 && j < 50 && i == 22))) 
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			} // first floor
			else if (((j > 51 && j < 75 && i == 23) || (j > 51 && j < 75 && i == 22)) || ((j > 76 && j < 98 && i == 23) || (j > 76 && j < 98 && i == 22)))
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			}
			else if (((j > 1 && j < 25 && i == 19) || (j > 1 && j < 25 && i == 18)) || ((j > 26 && j < 50 && i == 19) || (j > 26 && j < 50 && i == 18))) 
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			} // second floor
			else if ((j > 51 && j < 75 && i == 19) || (j > 51 && j < 75 && i == 18) || (j > 86 && j < 98 && i == 20))
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			}
			else if ((j > 76 && j < 98 && i == 19) || (j > 76 && j < 87 && i == 18))
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			}
			else if (((j > 1 && j < 15 && i == 15) || (j > 1 && j < 15 && i == 14)) || ((j > 16 && j < 30 && i == 15) || (j > 16 && j < 30 && i == 14))) 
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			} // third floor
			else if (((j > 31 && j < 75 && i == 15) || (j > 31 && j < 75 && i == 14)) || ((j > 76 && j < 98 && i == 15) || (j > 76 && j < 98 && i == 14)))
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			}
			else if (((j > 1 && j < 25 && i == 11) || (j > 1 && j < 25 && i == 10)) || ((j > 26 && j < 40 && i == 11) || (j > 26 && j < 40 && i == 10))) 
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			} // four floor
			else if (((j > 41 && j < 65 && i == 11) || (j > 41 && j < 65 && i == 10)) || ((j > 66 && j < 78 && i == 11) || (j > 66 && j < 78 && i == 10)))
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			}
			else if ((j > 79 && j < 98 && i == 11) || (j > 79 && j < 98 && i == 10))
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			}
			else if (((j > 1 && j < 15 && i == 7) || (j > 1 && j < 15 && i == 6)) || ((j > 16 && j < 35 && i == 7) || (j > 16 && j < 35 && i == 6)))
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			} // fifth floor
			else if (((j > 36 && j < 59 && i == 7) || (j > 36 && j < 59 && i == 6)) || ((j > 60 && j < 78 && i == 7) || (j > 60 && j < 78 && i == 6)))
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			}
			else if ((j > 79 && j < 98 && i == 7) || (j > 79 && j < 98 && i == 6))
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			}
			else if (((j > 1 && j < 15 && i == 3) || (j > 1 && j < 15 && i == 2)) || ((j > 16 && j < 35 && i == 3) || (j > 16 && j < 35 && i == 2)))
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			} // sixth floor
			else if (((j > 36 && j < 59 && i == 3) || (j > 36 && j < 59 && i == 2)) || ((j > 60 && j < 78 && i == 3) || (j > 60 && j < 78 && i == 2)))
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			}
			else if ((j > 79 && j < 98 && i == 3) || (j > 79 && j < 98 && i == 2))
			{
				table[j][i] = '#';
				printf("%c", table[j][i]);
			}
			else if (j == enemyX && i == enemyY) //print enemies
			{
				table[j][i] = '&';
				printf("%c", table[j][i]);
			}
			else if (j == enemyX2 && i == enemyY2)
			{
				table[j][i] = '&';
				printf("%c", table[j][i]);
			}
			else if (j == enemyX3 && i == enemyY3)
			{
				table[j][i] = '&';
				printf("%c", table[j][i]);
			}
			else if (j == enemyX4 && i == enemyY4)
			{
				table[j][i] = '&';
				printf("%c", table[j][i]);
			}
			else if (j == enemyX5 && i == enemyY5)
			{
				table[j][i] = '&';
				printf("%c", table[j][i]);
			}
			else if (j == enemyX6 && i == enemyY6)
			{
				table[j][i] = '&';
				printf("%c", table[j][i]);
			}
			else if (j == enemyX7 && i == enemyY7)
			{
				table[j][i] = '&';
				printf("%c", table[j][i]);
			}
			else if (j == enemyX8 && i == enemyY8)
			{
				table[j][i] = '&';
				printf("%c", table[j][i]);
			}
			else if (j == enemyX9 && i == enemyY9)
			{
				table[j][i] = '&';
				printf("%c", table[j][i]);
			}
			else if (j == enemyX10 && i == enemyY10)
			{
				table[j][i] = '&';
				printf("%c", table[j][i]);
			}
			else if (j == x && i == y) //print user
			{
				table[j][i] = usr;
				printf("%c", table[j][i]);
			}
			else if (j == fX && i == fY) // print fruit 1
			{
				if (fHits[0])
				{
					table[j][i] = ' ';
					printf("%c", table[j][i]);
				}
				else
				{
					table[j][i] = fruit;
					printf("%c", table[j][i]);
				}
			}
			else if (j == f2X && i == f2Y) // print fruit 2
			{
				if (fHits[1])
				{
					table[j][i] = ' ';
					printf("%c", table[j][i]);
				}
				else
				{
					table[j][i] = fruit;
					printf("%c", table[j][i]);
				}
			}
			else if (j == f3X && i == f3Y) // print fruit 3
			{
				if (fHits[2])
				{
					table[j][i] = ' ';
					printf("%c", table[j][i]);
				}
				else
				{
					table[j][i] = fruit;
					printf("%c", table[j][i]);
				}
			}
			else if (j == f4X && i == f4Y) // print fruit 4
			{
				if (fHits[3])
				{
					table[j][i] = ' ';
					printf("%c", table[j][i]);
				}
				else
				{
					table[j][i] = fruit;
					printf("%c", table[j][i]);
				}
			}
			else if (j == f5X && i == f5Y) // print fruit 5
			{
				if (fHits[4])
				{
					table[j][i] = ' ';
					printf("%c", table[j][i]);
				}
				else
				{
					table[j][i] = fruit;
					printf("%c", table[j][i]);
				}
			}
			else if (j == f6X && i == f6Y) // print fruit 6
			{
				if (fHits[5])
				{
					table[j][i] = ' ';
					printf("%c", table[j][i]);
				}
				else
				{
					table[j][i] = fruit;
					printf("%c", table[j][i]);
				}
			}
			else if (j == f7X && i == f7Y) // print fruit 7
			{
				if (fHits[6])
				{
					table[j][i] = ' ';
					printf("%c", table[j][i]);
				}
				else
				{
					table[j][i] = fruit;
					printf("%c", table[j][i]);
				}
			}
			else if (j == f8X && i == f8Y) // print fruit 8
			{
				if (fHits[7])
				{
					table[j][i] = ' ';
					printf("%c", table[j][i]);
				}
				else
				{
					table[j][i] = fruit;
					printf("%c", table[j][i]);
				}
			}
			else if (j == f9X && i == f9Y) // print fruit 9
			{
				if (fHits[8])
				{
					table[j][i] = ' ';
					printf("%c", table[j][i]);
				}
				else
				{
					table[j][i] = fruit;
					printf("%c", table[j][i]);
				}
			}
			else if (j == f10X && i == f10Y) // print fruit 9
			{
				if (fHits[9])
				{
					table[j][i] = ' ';
					printf("%c", table[j][i]);
				}
				else
				{
					table[j][i] = fruit;
					printf("%c", table[j][i]);
				}
			}
			else if ((table[x][y] == fruit) && x == fX) // mark if youser hit 1 fruit
			{
				hitFruit = 1;
				fHits[0] = true;
			}
			else if ((table[x][y] == fruit) && x == f2X) // mark if youser hit 2 fruit
			{
				hitFruit = 2;
				fHits[1] = true;
			}
			else if ((table[x][y] == fruit) && x == f3X) // mark if youser hit 3 fruit
			{
				hitFruit = 3;
				fHits[2] = true;
			}
			else if ((table[x][y] == fruit) && x == f4X) // mark if youser hit 4 fruit
			{
				hitFruit = 4;
				fHits[3] = true;
			}
			else if ((table[x][y] == fruit) && x == f5X) // mark if youser hit 5 fruit
			{
				hitFruit = 5;
				fHits[4] = true;
			}
			else if ((table[x][y] == fruit) && x == f6X) // mark if youser hit 6 fruit
			{
				hitFruit = 6;
				fHits[5] = true;
			}
			else if ((table[x][y] == fruit) && x == f7X) // mark if youser hit 7 fruit
			{
				hitFruit = 7;
				fHits[6] = true;
			}
			else if ((table[x][y] == fruit) && x == f8X) // mark if youser hit 8 fruit
			{
				hitFruit = 8;
				fHits[7] = true;
			}
			else if ((table[x][y] == fruit) && x == f9X) // mark if youser hit 9 fruit
			{
				hitFruit = 9;
				fHits[8] = true;
			}
			else if ((table[x][y] == fruit) && x == f10X) // mark if youser hit 10 fruit
			{
				hitFruit = 10;
				fHits[9] = true;
			}
			else
			{
				table[j][i] = '\0'; 
				printf("%c", table[j][i]);
			}
			if (table[x][y] == '&') // if enemy hit user
			{
				gameOver = true;
				die = true;
			}
				
		}
		printf("\n");
	}

	//bottom texture line
	for (int i = 0; i < width; i++)
	{
		printf("#");
	}
	printf("\n");
}
//initialize moving
void move()
{
	if (_kbhit())
	{
		switch (_getch())
		{
		case KEY_UP:
			y--;
			if (!inspectionMoving())
				y++;
			if (y < 1)
				y++;
			break;
		case KEY_DOWN:
			y++;
			if (!inspectionMoving())
				y--;
			if (y > (height - 1))
				y--;
			break;
		case KEY_LEFT:
			x--;
			if (!inspectionMoving())
				x++;
			if (x < 1)
				x = 1;
			break;
		case KEY_RIGHT:
			x++;
			if (!inspectionMoving())
				x--;
			if (x > (width - 2))
				x = width - 2;
			break;
		case 'q':
			gameOver = true;
			break;
		}
	}
}
void moveEnemy()
{
	if (enemyX2 == 40)
		k2 = -1;
	if (enemyX2 == 1)
		k2 = 1;
	if (enemyX == 40)
		k = -1;
	if (enemyX == 1)
		k = 1;
	if (enemyX3 == 98)
		k3 = -1;
	if (enemyX3 == 50)
		k3 = 1;
	if (enemyX4 == 98)
		k4 = -1;
	if (enemyX4 == 50)
		k4 = 1;
	if (enemyX5 == 48)
		k5 = -1;
	if (enemyX5 == 20)
		k5 = 1;
	if (enemyX6 == 48)
		k6 = -1;
	if (enemyX6 == 20)
		k6 = 1;
	if (enemyX7 == 90)
		k7 = -1;
	if (enemyX7 == 60)
		k7 = 1;
	if (enemyX8 == 48)
		k8 = -1;
	if (enemyX8 == 10)
		k8 = 1;
	if (enemyX9 == 45)
		k9 = -1;
	if (enemyX9 == 10)
		k9 = 1;
	if (enemyX10 == 40)
		k10 = -1;
	if (enemyX10 == 10)
		k10 = 1;

	enemyX += k;
	enemyX2 += k2;
	enemyX3 += k3;
	enemyX4 += k4;
	enemyX5 += k5;
	enemyX6 += k6;
	enemyX7 += k7;
	enemyX8 += k8;
	enemyX9 += k9;
	enemyX10 += k10;
}
// proverka na texturu
bool inspectionMoving()
{
	if (table[x][y] == '#')
		return false;
	else
		return true;
}
//if u die young :)
void massage()
{
	system("cls");
	printf("\n\n");
	for (int i = 1; i < 6; i++)
	{
		printf("\t");
		for (int j = 1; j < 74; j++)
		{
			if (i == 1 && (j == 1 || j == 7 || j == 10 || j == 11 || j == 12 || j == 15 || j == 19 || j == 24 || j == 25 || j == 26 || j == 27 || j == 30 || j == 31 || j == 32 || j == 33 || j == 38 || j == 43 || j == 44 || j == 45 || j == 46 || j == 52 || j == 53 || j == 57 || j == 58 || j == 63 || j == 68 || j == 69 || j == 73))
				printf("#");
			else if (i == 2 && (j == 2 || j == 6 || j == 9 || j == 13 || j == 15 || j == 19 || j == 24 || j == 28 || j == 30 || j == 37 || j == 39 || j == 43 || j == 47 || j == 52 || j == 54 || j == 56 || j == 58 || j == 62 || j == 64 || j == 68 || j == 70 || j == 73))
				printf("#");
			else if (i == 3 && (j == 3 || j == 5 || j == 9 || j == 13 || j == 15 || j == 19 || j == 24 || j == 28 || j == 30 || j == 31 || j == 32 || j == 33 || j == 36 || j == 40 || j == 43 || j == 47 || j == 52 || j == 55 || j == 58 || j == 61 || j == 65 || j == 68 || j == 71 || j == 73))
				printf("#");
			else if (i == 4 && (j == 4 || j == 9 || j == 13 || j == 15 || j == 19 || j == 24 || j == 28 || j == 30 || j == 35 || j == 36 || j == 37 || j == 38 || j == 39 || j == 40 || j == 41 || j == 43 || j == 47 || j == 52 || j == 58 || j == 60 || j == 61 || j == 62 || j == 63 || j == 64 || j == 65 || j == 66 || j == 68 || j == 72 || j == 73))
				printf("#");
			else if (i == 5 && (j == 4 || j == 9 || j == 10 || j == 11 || j == 12 || j == 15 || j == 16 || j == 17 || j == 18 || j == 24 || j == 25 || j == 26 || j == 27 || j == 30 || j == 31 || j == 32 || j == 33 || j == 35 || j == 41 || j == 43 || j == 44 || j == 45 || j == 46 || j == 52 || j == 58 || j == 60 || j == 66 || j == 68 || j == 73))
				printf("#");
			else
				printf(" ");
		}
		printf("\n");
	}
	Sleep(1500); //pause while user reading...
}
void massageQuit()
{
	system("cls");
	printf("\n Closing..");
	Sleep(500);
}
