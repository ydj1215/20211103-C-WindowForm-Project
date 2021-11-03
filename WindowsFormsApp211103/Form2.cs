using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp211103
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            Text = "부품 관리";

            dataGridView1.DataSource = DataManager.Books;
            dataGridView1.CurrentCellChanged += dataGridView1_CurrentCellChanged;
            button1.Click += (sender, e) =>
            {
                //추가
                try
                {
                    if (DataManager.Books.Exists(x => x.Isbn == textBox1.Text))
                    {
                        MessageBox.Show("이미 목록에 존재하는 부품입니다.");
                    }
                    else
                    {
                        Book book = new Book()
                        {
                            Isbn = textBox1.Text,
                            Name = textBox2.Text,
                            Publisher = textBox3.Text,
                            Page = int.Parse(textBox4.Text)
                        };

                        DataManager.Books.Add(book);

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = DataManager.Books;
                        DataManager.Save();

                    }
                }
                catch (Exception ex)
                {

                }
            };

            button2.Click += (sender, e) =>
            {
                //수정
                try
                {
                    Book book = DataManager.Books.Single(x => x.Isbn == textBox1.Text);
                    book.Name = textBox2.Text;
                    book.Publisher = textBox3.Text;
                    book.Page = int.Parse(textBox4.Text);

                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = DataManager.Books;
                    DataManager.Save();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("목록에 존재하지 않는 부품입니다.");
                }
            };

            button3.Click += (sender, e) =>
            {
                //삭제
                try
                {
                    Book book = DataManager.Books.Single(x => x.Isbn == textBox1.Text);
                    DataManager.Books.Remove(book);

                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = DataManager.Books;
                    DataManager.Save();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("목록에 존재하지 않는 부품입니다.");
                }
            };

            button4.Click += (sender, e) =>
            {
                //검색
                try
                {
                    Book book = DataManager.Books.Single(x => x.Name == textBox5.Text);
                    MessageBox.Show("목록에 입력된 부품입니다.");
                    string str = book.Name + "";
                    string str2 = book.Page + "";
                    string str3 = book.Isbn + "";
                    MessageBox.Show(str + " 의 가격은 " + str2 + " 원이고 일련번호는 " + str3 + " 입니다." );

                }
                catch (Exception ex)
                {
                    MessageBox.Show("입력되지 않은 부품입니다.");
                }
            };
        }
        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                Book book = dataGridView1.CurrentRow.DataBoundItem as Book;
                textBox1.Text = book.Isbn;
                textBox2.Text = book.Name;
                textBox3.Text = book.Publisher;
                textBox4.Text = book.Page.ToString();
            }

            catch (Exception ex)
            {

            }
        }
    }
}
