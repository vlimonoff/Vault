using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vault
{
    public partial class Form1 : Form
    {
        public Bitmap HandlerTexture = Resource1.handle;

        public Form1()
        {
            InitializeComponent();
            panel2.Controls.Add(pictureBox1);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void handleCountTrackBar_Scroll(object sender, EventArgs e)
        {
            handleCounterLabel.Text = "Ручек: " + Math.Pow(handleCountTrackBar.Value, 2).ToString();
        }

        // Функция нажатия на кнопку рестарта
        private void RestartButton_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Resource1.closed;   // Выводим на "мониторчик" сообщение "CLOSED"
            pictureBox1.Controls.Clear();           // Удаляем иконки с pictureBox1
            pictureBox1.Visible = false;            // Устанавливаем невидимость pictureBox1
            StartGame();                            // Вызываем функцию старта игры
            pictureBox1.Visible = true;             // Устанавливаем видимость pictureBox1
        }

        private Handler[,] handlers;

        // Функция старта игры
        public void StartGame()
        {
            int N = handleCountTrackBar.Value;          // Определяем размерность 
            handlers = new Handler[N, N];               // Перезаполняем массив ручек
            int delta = pictureBox1.Width / N;          // Определяем размер изображения

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    handlers[i, j] = new Handler();                                     // Инициализируем каждый элемент массива
                    handlers[i, j].position = new Point(i, j);                          // Запоминаем в его поле position его колонку и столбец
                    handlers[i, j].picture.Size = new Size(delta, delta);               // Устанавливаем размер иконки ручки, вычисленный ранее
                    handlers[i, j].picture.Location = new Point(i * delta, j * delta);  // Устанавливаем местоположение иконки
                    pictureBox1.Controls.Add(handlers[i, j].picture);                   // Добавляем иконку ручки в качестве элемента pictureBox1
                    handlers[i, j].picture.Click += hdl_Click;                          // Определяем функцию, вызываемую при клике на иконку ручки
                }
            }

            Boolean isCorrectForPlay = false;                   // Инициализируем флаг для проверки на корректность
            while (!isCorrectForPlay)                           // Пока флаг не примет значение true, будем выполнять следующее:
            {
                Random r = new Random();
                int turnsCount = r.Next(N * N - 1) + 1;         // Определяем случайное количество поворотов (от 0 до N^2)

                while (turnsCount > 0)                                                 // Осуществляем поворот turnsCount ручек
                {                                                                      // Для этого определяем случайную ручку и
                    Handler randomHandler = handlers[r.Next(0, N), r.Next(0, N)];      // поворачиваем стоящие с ней на одной строке
                    foreach (Handler h in handlers)                                    // или в одном столбце.
                    {
                        if (h.position.X == randomHandler.position.X || h.position.Y == randomHandler.position.Y) h.Reverse();
                    }
                    turnsCount--;
                }

                Boolean firstState = handlers[0, 0].state;           // Определяем состояние первой ручки (вертикальное или горизонтальное)
                foreach (Handler h in handlers)
                {
                    if (!isCorrectForPlay && h.state != firstState) // Проверяем, корректна ли ситуация на доске, то есть
                    {                                               // не получилось ли так, что пользователю отобразится уже
                        isCorrectForPlay = true;                    // решенная головоломка. Если хотя бы одна ручка отличается
                        break;                                      // от положения первой, значит, все корректно. 
                    }
                }
            }
        }

        public void hdl_Click(object sender, EventArgs e)
        {
            if (isUserWin(sender, e)) pictureBox2.Image = Resource1.open;   // Если игрок выиграл - отображаем на мониторчике OPEN
            else pictureBox2.Image = Resource1.closed;                      // иначе - отображаем CLOSED
        }

        // Функция проверки на то, выигрышная ли на данный момент позиция на доске.
        private Boolean isUserWin(object sender, EventArgs a)
        {
            Handler senderHandler = new Handler();          // Инициализируем новый объект типа Handler

            foreach (Handler h in handlers)                 // Перебираем все наши ручки,
            {                                               // пока не найдем ту, по иконке которой игрок кликнул
                if (sender == h.picture)                    
                {
                    senderHandler = h;                      // Когда такая нашлась (а она точно найдется), кладем ее
                    break;                                  // в senderHandler и выходим из цикла. 
                }
            }

            foreach (Handler h in handlers)                 // Поворачиваем все ручки, у которых строка или столбец
            {                                               // совпадает с той, по которой игрок кликнул.
                if (h.position.X == senderHandler.position.X || h.position.Y == senderHandler.position.Y) h.Reverse();
            }

            Boolean firstState = handlers[0, 0].state;      // Запоминаем положение первой ручки
                
            foreach (Handler h in handlers)                 // Сравниваем с ним значения каждой ручки до тех пор,
            {                                               // пока не найдется ручка с отличным от первой положением.
                if (h.state != firstState) return false;    // Если такая ручка нашлась - игрок не выиграл, функция
            }                                               // возвращает false.

            return true;                                    // Если мы не вышли из функции раньше, значит игрок выиграл - true.
        }
    }
}
