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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text = "컴퓨터 조립 부품 구매";

            //라벨에 숫자 나오게
            label1.Text = DataManager.Books.Count.ToString(); //전체 부품수
            label9.Text = DataManager.Users.Count.ToString(); //사용자 수
            label11.Text = DataManager.Books.Where((x) => x.IsBorrowed).Count().ToString(); //구매한 부품의 개수
            label10.Text = (DataManager.Books.Count - DataManager.Books.Where((x) => x.IsBorrowed).Count()).ToString(); //구매한 부품의 개수

            //데이터 그리드에 정보 나오게
            dataGridView1.DataSource = DataManager.Books;
            dataGridView2.DataSource = DataManager.Users;
            dataGridView1.CurrentCellChanged += dataGridView1_CurrentCellChanged;
            dataGridView2.CurrentCellChanged += dataGridView2_CurrentCellChanged;

            //버튼 이벤트 설정
            button1.Click += button1_Click;
            button2.Click += button2_Click;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //구매
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("일련번호를 입력해주세요.");
            }

            else if (textBox3.Text.Trim() == "")
            {
                MessageBox.Show("구매자 ID를 입력해주세요.");
            }

            else
            {
                try
                {
                    //where조건에 맞는 하나만을 선택할 때 쓰는 Single 메서드
                    Book book = DataManager.Books.Single(x => x.Isbn == textBox1.Text);
                    if (book.IsBorrowed)
                    {
                        //이미 구매된 부품이라면
                        MessageBox.Show("이미 판매된 부품입니다!");
                    }
                    else
                    {
                        //아직 구매가 되지 않았다면
                        User user = DataManager.Users.Single(x => x.Id == int.Parse(textBox3.Text));
                        book.UserId = user.Id.ToString();
                        book.UserName = user.Name;
                        book.IsBorrowed = true;
                        book.BorrowedAt = DateTime.Now;

                        if (user.Money >= 0)
                        {
                            user.Money -= book.Page;
                        }
                        else
                        {
                            MessageBox.Show("보유 금액이 부족합니다!");
                            user.Money -= book.Page;
                            string str = user.Money.ToString() + " 원 만큼의 금액이 부족합니다!";
                            MessageBox.Show(str);
                        }

                        //변경되었음을 지정해주기 위한 리프레쉬 작업
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = DataManager.Books;

                        dataGridView2.DataSource = null;
                        dataGridView2.DataSource = DataManager.Users;

                        //파일에도 바뀐 내용을 저장
                        DataManager.Save();

                        MessageBox.Show("\"" + book.Name + "\"이/가" + user.Name + "\"님께 구매되었습니다.");

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("존재하지 않는 부품 또는 사용자입니다.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //취소
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("일련번호를 입력해주세요.");
            }
            else
            {
                try
                {
                    Book book = DataManager.Books.Single(x => x.Isbn == textBox1.Text);
                    if (book.IsBorrowed)
                    {
                        User user = DataManager.Users.Single(x => x.Id.ToString() == textBox3.Text);
                        book.UserId = "";
                        book.UserName = "";
                        book.IsBorrowed = false;
                        book.BorrowedAt = new DateTime();

                        user.Money += book.Page;

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = DataManager.Books;
                        dataGridView2.DataSource = null;
                        dataGridView2.DataSource = DataManager.Users;

                        DataManager.Save();
                    }
                    else
                    {
                        MessageBox.Show("구매 상태가 아닙니다.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("존재하지 않는 부품 또는 사용자입니다.");
                }
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                Book book = dataGridView1.CurrentRow.DataBoundItem as Book;
                textBox1.Text = book.Isbn;
                textBox2.Text = book.Name;
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridView2_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                User user = dataGridView2.CurrentRow.DataBoundItem as User;
                textBox3.Text = user.Id.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private void 부품관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           new Form2().ShowDialog();
        }
    }
}
