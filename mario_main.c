#define _CRT_SECURE_NO_WARNINGS

#include <windows.h>
#include <stdio.h>
#include <stdlib.h>


#define WIDTH 800
#define HEIGHT 600

//базовые размеры окна
int windowSizeX = WIDTH;
int windowSizeY = HEIGHT;
int windowX = 200;
int windowY = 100;
// размеры дочернего окна (юзера)
int usrSizeX = 40;
int usrSizeY = 50;
int usrX = WIDTH / 2;
int usrY = HEIGHT * 0.35;
//константы для проверки изменений размера окна
int widthLast = WIDTH;
int okno = 0;
//окно меина и юзера
HWND hwnd, usr;
HCURSOR hCursor; // структура курсора

//prototype
LONG WINAPI WndProc(HWND, UINT, WPARAM, LPARAM);
void BackgroundDraw(HWND hwnd, int number);

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, // hInstance – дескриптор(описание) процесса(instance handle) – число, идентифицирующее программу, когда она работает под Windows.Если одновременно работают несколько копий одной программы, каждая копия имеет свое значение hInstance.
	LPSTR lpCmdLine, int nCmdShow) // szCmdLine — указатель на оканчивающуюся нулем строку, в которой содержатся параметры, переданные в программу из командной строки.Можно запустить программу с параметром командной строки, вставив этот параметр после имени программы в командной строке.
{

	MSG msg;	// структура сообщения
	WNDCLASS w; // структура класса окна
				// Регистрация класса окна
	memset(&w, 0, sizeof(WNDCLASS));
	w.style = CS_HREDRAW | CS_VREDRAW; // стили отображения окна
	w.lpfnWndProc = WndProc; // имя оконной функции (lpfnWndProc - указатель на оконную процедуру)
	w.hInstance = hInstance;
	w.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
	w.lpszClassName = L"My Class";
	RegisterClass(&w);
	// Создание окна
	hwnd = CreateWindow(L"My Class", L"street fighter II", WS_OVERLAPPEDWINDOW, windowX, windowY, windowSizeX, windowSizeY, NULL, NULL, hInstance, NULL);
	RECT Win;
	GetClientRect(hwnd, &Win);
	// стартовое положение юзера
	usrX = Win.right * 0.5;
	usrY = Win.bottom * 0.37;
	usr = CreateWindow(L"STATIC", NULL, WS_CHILD | WS_VISIBLE | WS_BORDER, usrX, usrY, usrSizeX, usrSizeY, hwnd, 0, hInstance, NULL);
	//создаем курсор
	hCursor = LoadCursorFromFile(L"C:\\Users\\Volodymyr\\Documents\\Visual Studio 2015\\Projects\\images\\cursor_default.ani");
	if (hCursor == NULL)
	{
		MessageBox(hwnd, L"Cant found cursor", L"Load image", MB_ICONWARNING | MB_OK);
	}
	SetCursor(hCursor);



	ShowWindow(hwnd, nCmdShow); // отображение
	UpdateWindow(hwnd);			// перерисовка

	ShowWindow(usr, nCmdShow); // отображение
	UpdateWindow(usr);

						// Цикл обработки сообщений
	while (GetMessage(&msg, NULL, 0, 0)) //Получаем сообщение из очереди
	{
		TranslateMessage(&msg);
		DispatchMessage(&msg); //Передаём сообщение соответствующей функции окна на обработку
		
		if (VK_ESCAPE == msg.wParam)
            break;
	}
	return msg.wParam;
}

LONG WINAPI WndProc(HWND hwnd, UINT Message, WPARAM wparam, LPARAM lparam)
{
	RECT Rect;
	GetClientRect(hwnd, &Rect);
	//константа okno - принимает значение 1 если размер окна был изменён
	if ((Rect.right != widthLast))
	{
		widthLast = Rect.right;
		okno = 1;
	}
	
	int moveWindow = 10;
	//Цикл обработки сообщений
	switch (Message)
	{
		//если нажата клавиша
	case WM_KEYDOWN: 
		//проверка на изменение размеров окна
		if (okno == 1)
		{
			usrX = Rect.right * 0.5;
			usrY = Rect.bottom * 0.37;
			UpdateWindow(usr);
			/* вывод инфо при изменении размера окна
			wchar_t errWidth[] = L"Rect width is %i";
			wchar_t terk[512];
			swprintf(terk, wcslen(terk), errWidth, Rect.right);
			MessageBox(hwnd, terk, L"Load image", MB_ICONWARNING | MB_OK);
			*/
			widthLast = Rect.right;
			okno = 0;
		}
		
		//цикл обработки движений юзера
		switch (wparam) {
		case VK_LEFT:
			if ((usrX <= (Rect.right * 0.5)) && (usrY > Rect.bottom * 0.38) && (usrY < Rect.bottom * 0.7))
				break;
			else if ((usrX <= Rect.right * 0.1) && (usrY > Rect.bottom * 0.7))
				break;
			else if (usrX > Rect.left)
			{
				MoveWindow(usr, (usrX -= moveWindow), usrY, usrSizeX, usrSizeY, TRUE);
				UpdateWindow(usr);
			}
			break;
		case VK_RIGHT:
			if ((usrX >= (Rect.right * 0.89)) && (usrX <= (Rect.right * 0.95)) && (usrY > Rect.bottom * 0.7))
				break;
			else if ((usrX + usrSizeX + moveWindow) < Rect.right)
			{
				MoveWindow(usr, (usrX += moveWindow), usrY, usrSizeX, usrSizeY, TRUE);
				UpdateWindow(usr);
			}			
			break;
		case VK_UP:
			if (usrY > Rect.top)
			{
				int i = 10, j = 10;
				while (i != 0)
				{
					MoveWindow(usr, usrX, (usrY -= moveWindow), usrSizeX, usrSizeY, TRUE);
					UpdateWindow(usr);
					if (GetAsyncKeyState(VK_UP) && GetAsyncKeyState(VK_RIGHT))
					{
						if ((usrX + usrSizeX + moveWindow) < Rect.right)
						{
							MoveWindow(usr, (usrX += 10), usrY, usrSizeX, usrSizeY, TRUE);
							UpdateWindow(usr);
						}
					}
					else if (GetAsyncKeyState(VK_UP) && GetAsyncKeyState(VK_LEFT))
					{
						if (usrX > Rect.left)
						{
							MoveWindow(usr, (usrX -= 10), usrY, usrSizeX, usrSizeY, TRUE);
							UpdateWindow(usr);
						}
					}
					UpdateWindow(hwnd);
					Sleep(20);
					i -= 1;
				}
				while (j != 0)
				{
					MoveWindow(usr, usrX, (usrY += moveWindow), usrSizeX, usrSizeY, TRUE);
					UpdateWindow(usr);
					if (GetAsyncKeyState(VK_UP) && GetAsyncKeyState(VK_RIGHT))
					{
						if ((usrX + usrSizeX + moveWindow) < Rect.right)
						{
							MoveWindow(usr, (usrX += 10), usrY, usrSizeX, usrSizeY, TRUE);
							UpdateWindow(usr);
						}
					}
					else if (GetAsyncKeyState(VK_UP) && GetAsyncKeyState(VK_LEFT))
					{
						if (usrX > Rect.left)
						{
							MoveWindow(usr, (usrX -= 10), usrY, usrSizeX, usrSizeY, TRUE);
							UpdateWindow(usr);
						}
					}
					UpdateWindow(hwnd);
					j -= 1;
					Sleep(20);
				}
			}
			break;
		case VK_DOWN:
			if ((usrX >= (Rect.right * 0.5)) && (usrX <= (Rect.right * 0.52)) && (usrY < Rect.bottom * 0.58))
			{
				int j = 14;
				while (j != 0)
				{
					MoveWindow(usr, usrX, (usrY += moveWindow), usrSizeX, usrSizeY, TRUE);
					UpdateWindow(usr);
					if (GetAsyncKeyState(VK_UP) && GetAsyncKeyState(VK_RIGHT))
					{
						if ((usrX + usrSizeX + moveWindow) < Rect.right)
						{
							MoveWindow(usr, (usrX += 10), usrY, usrSizeX, usrSizeY, TRUE);
							UpdateWindow(usr);
						}
					}
					else if (GetAsyncKeyState(VK_UP) && GetAsyncKeyState(VK_LEFT))
					{
						if (usrX > Rect.left)
						{
							MoveWindow(usr, (usrX -= 10), usrY, usrSizeX, usrSizeY, TRUE);
							UpdateWindow(usr);
						}
					}
					UpdateWindow(hwnd);
					j -= 1;
					Sleep(20);
				}
			}
			else if ((usrX >= (Rect.right * 0.86)) && (usrX <= (Rect.right * 0.88)) && (usrY > Rect.bottom * 0.55) && (usrY < Rect.bottom * 0.85))
			{
				int j = 14;
				while (j != 0)
				{
					MoveWindow(usr, usrX, (usrY += moveWindow), usrSizeX, usrSizeY, TRUE);
					UpdateWindow(usr);
					if (GetAsyncKeyState(VK_UP) && GetAsyncKeyState(VK_RIGHT))
					{
						if ((usrX + usrSizeX + moveWindow) < Rect.right)
						{
							MoveWindow(usr, (usrX += 10), usrY, usrSizeX, usrSizeY, TRUE);
							UpdateWindow(usr);
						}
					}
					else if (GetAsyncKeyState(VK_UP) && GetAsyncKeyState(VK_LEFT))
					{
						if (usrX > Rect.left)
						{
							MoveWindow(usr, (usrX -= 10), usrY, usrSizeX, usrSizeY, TRUE);
							UpdateWindow(usr);
						}
					}
					UpdateWindow(hwnd);
					j -= 1;
					Sleep(20);
				}
			}
			break;
		default:
			break;
		}
		break;
	case WM_SETCURSOR: //сообщения от мыши при передвижении и обновление курсора в этой позиции
			if (LOWORD(wparam) == HTCLIENT)
			{
				SetCursor(hCursor);
				return TRUE;
			}
			break;
	case WM_PAINT: 
		// рисуем бг и юзера
		BackgroundDraw(hwnd, 0);
		BackgroundDraw(usr, 1);
		break;
	/*case WM_SIZE:  - попытки сделать окно на весь экран, чтобы перекрывало панель задач тоже
		if (state > 2)
		{
			ShowWindow(hwnd, SW_MAXIMIZE);
			MessageBox(hwnd, L"NU KAK TAAAAAK", L"Load image", MB_ICONWARNING | MB_OK);
			UpdateWindow(hwnd);
			state = 0;
		}
		break;*/
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hwnd, Message, wparam, lparam);
	}
	return 0;
}

void BackgroundDraw(HWND hwnd, int number)
{
	HDC hDC, hCompatibleDC;
	PAINTSTRUCT PaintStruct;
	HANDLE hBitmap, hOldBitmap;
	RECT Rect;
	BITMAP Bitmap;

	wchar_t tempPath[512];
	//путь к картинке с порядковым номером
	wchar_t backgroundImg[] = L"C:\\Users\\Volodymyr\\Documents\\Visual Studio 2015\\Projects\\images\\IMG%i.BMP";
	swprintf(tempPath, wcslen(tempPath), backgroundImg, number);

	//печать ошибки если не нашел путь к файлу или файл
	wchar_t errLoadBgImg[] = L"Can't found background image! %s";
	wchar_t temp[512];
	swprintf(temp, wcslen(temp), errLoadBgImg, backgroundImg);

	//малюем
	hDC = BeginPaint(hwnd, &PaintStruct);
	// Загружаем bitmap, который будет отображаться в окне, из файла.
	hBitmap = LoadImage(NULL, tempPath, IMAGE_BITMAP, 0, 0, LR_LOADFROMFILE);
	if (hBitmap == NULL)
	{
		MessageBox(hwnd, temp, L"Load image", MB_ICONWARNING | MB_OK);
	}
	// получаем размероность загруженного bitmap'a.
	GetObject(hBitmap, sizeof(BITMAP), &Bitmap);
	// создаем совместный с контекстом окна контекст в памяти
	hCompatibleDC = CreateCompatibleDC(hDC);
	// делаем загруженный из фаила bitmap текущим в совместимом контексте
	hOldBitmap = SelectObject(hCompatibleDC, hBitmap);
	// определяем размер рабочей области окна
	GetClientRect(hwnd, &Rect);
	// копируем bitmap с совместимого на основной контекст с масштабированием
	StretchBlt(hDC, 0, 0, Rect.right, Rect.bottom, hCompatibleDC, 0, 0, Bitmap.bmWidth, Bitmap.bmHeight, SRCCOPY);
	// вновь делаем старый bitmap текущим
	SelectObject(hCompatibleDC, hOldBitmap);
	// удаляем загруженный с диска bitmap
	DeleteObject(hBitmap);
	// удаляем совместный контекст
	DeleteDC(hCompatibleDC);
	// освобождаем основной контекст, завершая перерисовку рабочей области окна
	EndPaint(hwnd, &PaintStruct);
}