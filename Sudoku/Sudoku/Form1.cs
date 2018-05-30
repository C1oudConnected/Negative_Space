using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        int count; // Кол-во не_пустых ячеек.
        object prev; // Значение ячейки до изменения.
        List<Error> errors = new List<Error>(); // Коллекция ошибок в виде пары повторяющихся элементов.
        bool isReady; // Индикатор завершенной автогенерации поля.

        public Form1() 
        {
            InitializeComponent();
            gameField.RowCount = 9;
            gameField.ColumnCount = 9;
            gameField.ScrollBars = ScrollBars.None;
            gameField.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gameField.DefaultCellStyle.Font = new Font(gameField.DefaultCellStyle.Font.FontFamily, 25);
            for (int i = 0; i < 9; i++)
            {
                gameField.Columns[i].Width = (gameField.Size.Width) / 9;
                gameField.Rows[i].Height = (gameField.Size.Height) / 9;
            }
            generateField(1);
        } // Создание поля и запуск его генерации.

        class Error
        {
            public int[] code;
            public Error(int[] _code)
            {
                code = _code;
            }
        } // Класс, представляющий из себя пару "координат" - ячеек, некорректно имеющих равные значения.

        void generateField(int lvl)
        {
            isReady = false;
            errors.Clear();
            count = 0;
            label1.Text = "";            
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    gameField.Rows[i].Cells[j].Value = null;
                    gameField.Rows[i].Cells[j].ReadOnly = false;
                    gameField.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                    if ((3 * (i / 3) + j / 3) % 2 == 0) gameField.Rows[i].Cells[j].Style.BackColor = Color.Aqua;
                    else gameField.Rows[i].Cells[j].Style.BackColor = Color.Aquamarine;
                }
            for (int i = 0; i < 50 - 12 * lvl; i++)
            {
                int a = generateCell();
                gameField.Rows[a / 9].Cells[a % 9].Value = (((a / 9) * 3 + (a / 9) / 3 + (a % 9)) % 9 + 1); count++;
            }
            Random r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                shuffleField(r);
            }
            for (int i = 0; i < 81; i++)
            {
                if (gameField.Rows[i / 9].Cells[i % 9].Value != null)
                {
                    gameField.Rows[i / 9].Cells[i % 9].ReadOnly = true;
                    gameField.Rows[i / 9].Cells[i % 9].Style.ForeColor = Color.DarkGreen;
                }
            }
            isReady = true;
        } // Генерация поля. Часично создается изначальная сетка по формуле, попутно увеличивая счетчик, кол-во созданных эелементов зависит от уровня сложности. Потом сетка (псевдо)случайно резмешивается.

        void shuffleField(Random r)
        {
            int i;
            switch (r.Next() % 4)
            {
                case 0:
                    i = shuffleLine(r);
                    swapLines(i / 9, i % 9);
                    break;
                case 1:
                    i = shuffleLine(r);
                    swapColumns(i / 9, i % 9);
                    break;
                case 2:
                    i = shuffleRow(r);
                    swapLines(3 * (i / 3), 3 * (i % 3));
                    swapLines(3 * (i / 3) + 1, 3 * (i % 3) + 1);
                    swapLines(3 * (i / 3) + 2, 3 * (i % 3) + 2);
                    break;
                case 3:
                    i = shuffleRow(r);
                    swapColumns(3 * (i / 3), 3 * (i % 3));
                    swapColumns(3 * (i / 3) + 1, 3 * (i % 3) + 1);
                    swapColumns(3 * (i / 3) + 2, 3 * (i % 3) + 2);
                    break;
            }

        } // Перетасовка поля.

        int shuffleLine(Random r)
        {
            int b = r.Next() % 9;
            int i;
            do
            {
                i = r.Next() % 3 + 3 * (b / 3);
            }
            while (i == b);
            return i * 9 + b;
        } // Поиск доступных к обмену линий (рядов или колонн).

        int shuffleRow(Random r)
        {
            int b = r.Next() % 3;
            int i;
            do
            {
                i = r.Next() % 3;
            }
            while (i == b);
            return 3 * i + b;
        } // Поиск доступных к обмену площадей (триад рядов или колонн).

        void swapLines(int a, int b)
        {
            for (int i = 0; i < 9; i++)
            {
                object buffer = gameField.Rows[a].Cells[i].Value;
                gameField.Rows[a].Cells[i].Value = gameField.Rows[b].Cells[i].Value;
                gameField.Rows[b].Cells[i].Value = buffer;
            }
        } // Обмен между двумя рядами.

        void swapColumns(int a, int b)
        {
            for (int i = 0; i < 9; i++)
            {
                object buffer = gameField.Rows[i].Cells[a].Value;
                gameField.Rows[i].Cells[a].Value = gameField.Rows[i].Cells[b].Value;
                gameField.Rows[i].Cells[b].Value = buffer;
            }
        } // Обмен между двумя колоннами.

        int generateCell()
        {
            Random rnd = new Random();
            int i;
            do
            {
                i = rnd.Next() % 81;
            }
            while (gameField.Rows[i / 9].Cells[i % 9].Value != null);
            return i;
        } // Получение случайной незанятой позиции на поле.


        //=========== Уровни сложности ============
        private void button1_Click(object sender, EventArgs e)
        {
            generateField(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            generateField(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            generateField(3);
        }


        //=========== Обработка хода игрока =======
        private void gameField_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isReady)
            {
                if (prev == null) count++;
                if (gameField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && Convert.ToInt32(gameField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) <= 9 && Convert.ToInt32(gameField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) > 0)
                     checkCell(e.RowIndex, e.ColumnIndex);
                else gameField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = prev;
                if (errors.Count == 0 && count == 81)
                    congrats();
            }
        } // Обработчик события смены значения ячейка. Вызов проверяющей и завершающей функции.

        void checkCell(int r, int c)
        {
            int startPt = 9 * 3 * (r / 3) + 3 * (c / 3);
            gameField.Rows[r].Cells[c].Style.BackColor = ((3 * (r / 3) + c / 3) % 2 == 0) ? Color.Aqua : Color.Aquamarine;
            for (int i = 0; i < 9; i++)
            {
                if (i != c && gameField.Rows[r].Cells[i].Value != null)
                    check(r, c, r, i);
                if (i != r && gameField.Rows[i].Cells[c].Value != null)
                    check(r, c, i, c);
                int coordinateCode = (startPt + 9 * (i / 3) + i % 3);

                if (coordinateCode != 9 * r + c && gameField.Rows[coordinateCode / 9].Cells[coordinateCode % 9].Value != null)
                    check(r, c, coordinateCode / 9, coordinateCode % 9);
            }
        } // Сравнивает ячейку с ее соседями по ряду, колонне и площади.

        void check(int r1, int c1, int r2, int c2)
        {
            int code1 = r1 * 9 + c1, code2 = r2 * 9 + c2;
            bool idx;
            if (Convert.ToInt32(gameField.Rows[r1].Cells[c1].Value) == Convert.ToInt32(gameField.Rows[r2].Cells[c2].Value))
            {
                gameField.Rows[r1].Cells[c1].Style.BackColor = Color.Red;
                gameField.Rows[r2].Cells[c2].Style.BackColor = Color.Red;
                errors.Add(new Error(new int[2] { code1, code2 }));
            }
            else
            {
                gameField.Rows[r2].Cells[c2].Style.BackColor = ((3 * (r2 / 3) + c2 / 3) % 2 == 0) ? Color.Aqua : Color.Aquamarine;
                for (int i = 0; i < errors.Count; i++)
                    if ((errors[i].code[0] == code2) || (errors[i].code[1] == code2))
                    {
                        idx = (errors[i].code[0] == code2) ? false : true;
                        if (errors[i].code[Convert.ToInt32(!idx)] == code1)
                            errors.RemoveAt(i);
                        else gameField.Rows[r2].Cells[c2].Style.BackColor = Color.Red;
                    }
            }
        } // Сравнивает две ячейки и отмечает ошибки.

        void congrats()
        { label1.Text = "Congratulations!"; } // Завершение игры, победа игрока, вызывается, когда не остается пустых ячеек поля и при этом отсутствуют ошибки.

        private void gameField_CellEnter(object sender, DataGridViewCellEventArgs e)
        { prev = gameField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value; } // Получвет значение ячейки перед редактирванием. Важно для того, чтобы в случае недопустимого редактирования обратить изменения, а также чтобы увеличивать счетчик отмеченных ячеек.
    }
}
