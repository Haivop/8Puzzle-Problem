using System.Runtime.CompilerServices;
using Cursova.AlgorithmRealizations;
using Cursova.Utilities;

namespace Cursova
{
    partial class UI
    {
        private System.ComponentModel.IContainer components = null;
        private int currentStepIndex = 0;
        private bool AStarAllowChange = true;
        private bool RBFSAllowChange = true;
        private List<Node> _path = null;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }
        
        private List<int> GetRandomMatrix()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                list.Add(i);
            }

            Shuffle(list);
            return list;
        }
        private static void Shuffle(List<int> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        private void GetRandomStart(object sender, EventArgs e)
        {
            var start = GetRandomMatrix();
            this.StartBoardBox1.Text = start[0].ToString();
            this.StartBoardBox2.Text = start[1].ToString();
            this.StartBoardBox3.Text = start[2].ToString();
            this.StartBoardBox4.Text = start[3].ToString();
            this.StartBoardBox5.Text = start[4].ToString();
            this.StartBoardBox6.Text = start[5].ToString();
            this.StartBoardBox7.Text = start[6].ToString();
            this.StartBoardBox8.Text = start[7].ToString();
            this.StartBoardBox9.Text = start[8].ToString();
        }
        public Node GetStartNode()
        {
            TextBox[,] startT = new TextBox[3, 3]; 
            TextBox[,] goalT = new TextBox[3, 3];

            startT[0, 0] = this.StartBoardBox1; 
            startT[0, 1] = this.StartBoardBox2; 
            startT[0, 2] = this.StartBoardBox3; 
            startT[1, 0] = this.StartBoardBox4; 
            startT[1, 1] = this.StartBoardBox5; 
            startT[1, 2] = this.StartBoardBox6; 
            startT[2, 0] = this.StartBoardBox7; 
            startT[2, 1] = this.StartBoardBox8; 
            startT[2, 2] = this.StartBoardBox9; 
            
            goalT[0, 0] = this.GoalBoardBox1;
            goalT[0, 1] = this.GoalBoardBox2;
            goalT[0, 2] = this.GoalBoardBox3;
            goalT[1, 0] = this.GoalBoardBox4;
            goalT[1, 1] = this.GoalBoardBox5;
            goalT[1, 2] = this.GoalBoardBox6;
            goalT[2, 0] = this.GoalBoardBox7;
            goalT[2, 1] = this.GoalBoardBox8;
            goalT[2, 2] = this.GoalBoardBox9;

            int[,] start = GetMatrixFromText(startT);
            int[,] goal = GetMatrixFromText(goalT);
            
            if(start != null && goal != null)return new Node(start, goal);
            return null;
        }
        private int[,] IsValidMatrixes(bool[] IsValid, int[,] matrix)
        {
            for (var i = 0; i < IsValid.Length; i++)
            {
                if (!IsValid[i])
                {
                    switch (i)
                    {
                        case 0:
                            MessageBox.Show("Не введені усі елементи! Будь ласка, " +
                                            "заповніть підсвічені зеленим комірки.");
                            break;
                        case 1:
                            MessageBox.Show("В матриці сторонні символи! Вони стали червоним, щоб ви їх бачили.");
                            break;
                        case 2:
                            MessageBox.Show("Занадто великі числа! Вони стали червоними, щоб ви їх побачили.");
                            break;
                        case 3:
                            MessageBox.Show("В матриці два або більше однакових елементи!" +
                                            " Дублікати стали червоними, щоб ви могли Їх прибрати.");
                            break;
                    }
                    return null;
                }
            }
            return matrix;
        }
        private int[,] GetMatrixFromText(TextBox[,] textMatrix)
        {
            bool[] IsValid = [true, true, true, true];
            int[,] matrix = new int[3, 3];
            var size = textMatrix.GetLength(0);
            
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    textMatrix[row, col].ForeColor = Color.Black;
                    textMatrix[row, col].BackColor = DefaultBackColor;
                    if(textMatrix[row, col].Text == "")
                    {
                        textMatrix[row, col].BackColor = Color.GreenYellow;
                        IsValid[0] = false;
                    }
                    if (!int.TryParse(textMatrix[row, col].Text, out matrix[row, col]))
                    {
                        textMatrix[row, col].ForeColor = Color.Red;
                        IsValid[1] = false;
                    } 
                    if (matrix[row, col] > 8)
                    {
                        textMatrix[row, col].ForeColor = Color.Red;
                        IsValid[2] = false;
                    }
                }
            }
            if (HasDuplicates(matrix, textMatrix)) IsValid[3] = false;
            return IsValidMatrixes(IsValid, matrix);
        }
        private bool HasDuplicates(int[,] matrix, TextBox[,] textMatrix)
        {
            List<int> seenElements = new List<int>();

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    int currentElement = matrix[row, col];
                    if (seenElements.Contains(currentElement))
                    {
                        textMatrix[row, col].ForeColor = Color.Red;
                        return true;
                    }
                    seenElements.Add(currentElement);
                }
            }
            return false;
            
        }
        
        private void NeutralizeAStar(object sender, EventArgs e){
            if (AStarAllowChange)
            {
                AStarAllowChange = false;
                RBFSAllowChange = false;

                AStarCheckBox.Checked = true;
                RBFSCheckBox.Checked = false;

                AStarAllowChange = true;
                RBFSAllowChange = true;
            }
        }
        private void NeutralizeRBFS(object sender, EventArgs e){
            if (RBFSAllowChange)
            {
                AStarAllowChange = false;
                RBFSAllowChange = false;

                AStarCheckBox.Checked = false;
                RBFSCheckBox.Checked = true;

                AStarAllowChange = true;
                RBFSAllowChange = true;
            }
        }
        private void StartSearch(object sender, EventArgs e)
        {
            ResetDifficulty();
            if(_path != null && _path.Count != 0) _path.Clear();
            bool resolved = false;

            if (!AStarCheckBox.Checked && !RBFSCheckBox.Checked)
            {
                MessageBox.Show("Оберіть метод пошуку!");
                return;
            }
            
            Node node = GetStartNode();
            if (node != null)
            {
                if (node.IsGoal())
                {
                    MessageBox.Show("Стартова та Кінцева матриця однакові!");
                    _path = new List<Node>();
                    _path.Add(node);
                    this.StepsTakenTextBox.Text = "0";
                    this.IterationsTextBox.Text = "0";
                    this.NodesInMemoryTextBox.Text = "1";
                    resolved = true;
                }
                else if (node.IsSolvable())
                {
                    (_path, this.IterationsTextBox.Text, this.NodesInMemoryTextBox.Text) = 
                        (AStarCheckBox.Checked) ? A.Star(node) : R.BFS(node);
                    if (_path != null)
                    {
                        resolved = true;
                        _path.Insert(0, node);
                        this.StepsTakenTextBox.Text = (_path.Count - 1).ToString();
                    }
                }
                if (resolved) MessageBox.Show("Шлях знайдено");
                else MessageBox.Show("Цей паззл не має вирішення!");
                
                currentStepIndex = 0;
                SetCurrentStepBoard(resolved);
            }
        }
        private void SetCurrentStepBoard(bool resolved)
        {
            this.CurrentStepBoardBox1.Text = resolved ? _path[currentStepIndex].Board[0, 0].ToString() : "?";
            this.CurrentStepBoardBox2.Text = resolved ? _path[currentStepIndex].Board[0, 1].ToString() : "?";
            this.CurrentStepBoardBox3.Text = resolved ? _path[currentStepIndex].Board[0, 2].ToString() : "?";
            this.CurrentStepBoardBox4.Text = resolved ? _path[currentStepIndex].Board[1, 0].ToString() : "?";
            this.CurrentStepBoardBox5.Text = resolved ? _path[currentStepIndex].Board[1, 1].ToString() : "?";
            this.CurrentStepBoardBox6.Text = resolved ? _path[currentStepIndex].Board[1, 2].ToString() : "?";
            this.CurrentStepBoardBox7.Text = resolved ? _path[currentStepIndex].Board[2, 0].ToString() : "?";
            this.CurrentStepBoardBox8.Text = resolved ? _path[currentStepIndex].Board[2, 1].ToString() : "?";
            this.CurrentStepBoardBox9.Text = resolved ? _path[currentStepIndex].Board[2, 2].ToString() : "?";
        }
        
        private void ResetDifficulty()
        {
            this.IterationsTextBox.Text = null;
            this.NodesInMemoryTextBox.Text = null;
            this.StepsTakenTextBox.Text = null;
        }
        private void Reset(object sender, EventArgs e)
        {
            if(_path != null) _path = null;
            this.StartBoardBox1.Text = null;
            this.StartBoardBox2.Text = null;
            this.StartBoardBox3.Text = null;
            this.StartBoardBox4.Text = null;
            this.StartBoardBox5.Text = null;
            this.StartBoardBox6.Text = null;
            this.StartBoardBox7.Text = null;
            this.StartBoardBox8.Text = null;
            this.StartBoardBox9.Text = null;
            
            this.StartBoardBox1.ForeColor = DefaultForeColor;
            this.StartBoardBox2.ForeColor = DefaultForeColor;
            this.StartBoardBox3.ForeColor = DefaultForeColor;
            this.StartBoardBox4.ForeColor = DefaultForeColor;
            this.StartBoardBox5.ForeColor = DefaultForeColor;
            this.StartBoardBox6.ForeColor = DefaultForeColor;
            this.StartBoardBox7.ForeColor = DefaultForeColor;
            this.StartBoardBox8.ForeColor = DefaultForeColor;
            this.StartBoardBox9.ForeColor = DefaultForeColor;
            
            this.StartBoardBox1.BackColor = DefaultBackColor;
            this.StartBoardBox2.BackColor = DefaultBackColor;
            this.StartBoardBox3.BackColor = DefaultBackColor;
            this.StartBoardBox4.BackColor = DefaultBackColor;
            this.StartBoardBox5.BackColor = DefaultBackColor;
            this.StartBoardBox6.BackColor = DefaultBackColor;
            this.StartBoardBox7.BackColor = DefaultBackColor;
            this.StartBoardBox8.BackColor = DefaultBackColor;
            this.StartBoardBox9.BackColor = DefaultBackColor;
            
            this.GoalBoardBox1.Text = "1";
            this.GoalBoardBox2.Text = "2";
            this.GoalBoardBox3.Text = "3";
            this.GoalBoardBox4.Text = "4";
            this.GoalBoardBox5.Text = "5";
            this.GoalBoardBox6.Text = "6";
            this.GoalBoardBox7.Text = "7";
            this.GoalBoardBox8.Text = "8";
            this.GoalBoardBox9.Text = "0";

            this.CurrentStepBoardBox1.Text = null;
            this.CurrentStepBoardBox2.Text = null;
            this.CurrentStepBoardBox3.Text = null;
            this.CurrentStepBoardBox4.Text = null;
            this.CurrentStepBoardBox5.Text = null;
            this.CurrentStepBoardBox6.Text = null;
            this.CurrentStepBoardBox7.Text = null;
            this.CurrentStepBoardBox8.Text = null;
            this.CurrentStepBoardBox9.Text = null;
            
            AStarCheckBox.Checked = false;
            RBFSCheckBox.Checked = false;
            
            ResetDifficulty();
        }
        
        private void NextStepInPath(object sender, EventArgs e)
        {
            if (_path != null && currentStepIndex != _path.Count - 1)
            {
                currentStepIndex++;
                SetCurrentStepBoard(true);
            }
        }
        private void PrevStepInPath(object sender, EventArgs e)
        {
            if (_path != null && currentStepIndex != 0)
            {
                currentStepIndex--;
                SetCurrentStepBoard(true);
            }
        }
        private void SavePathToFile(object sender, EventArgs e)
        {
            if (_path != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog.FileName = "path";
                    saveFileDialog.DefaultExt = "txt";
                    saveFileDialog.AddExtension = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(filePath))
                            {
                                foreach (var step in _path)
                                {
                                    for (var y = 0; y < 3; y++)
                                    {
                                        for (var x = 0; x < 3; x++)
                                        {
                                            writer.Write($"{step.Board[y, x]}  ");
                                        }

                                        writer.Write("\n");
                                    }
                                    if (!step.IsEqual(_path.Last())) writer.Write("   |\n   |\n");
                                }
                            }

                            MessageBox.Show("Файл успішно збережено!", "Успіх",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Виникла помилка: {ex.Message}", "Помилка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else MessageBox.Show("Немає вирішення, щоб зберігати");
        }
        private void Quit(object sender, EventArgs e)
        {
            this.Close();
        }
        
        #region UIComponents    
        
        private void InitializeComponent()
        {
            this.StartBoard = new System.Windows.Forms.Label();
            
            this.StartBoardBox1 = new System.Windows.Forms.TextBox();
            this.StartBoardBox2 = new System.Windows.Forms.TextBox();
            this.StartBoardBox3 = new System.Windows.Forms.TextBox();
            this.StartBoardBox4 = new System.Windows.Forms.TextBox();
            this.StartBoardBox5 = new System.Windows.Forms.TextBox();
            this.StartBoardBox6 = new System.Windows.Forms.TextBox();
            this.StartBoardBox7 = new System.Windows.Forms.TextBox();
            this.StartBoardBox8 = new System.Windows.Forms.TextBox();
            this.StartBoardBox9 = new System.Windows.Forms.TextBox();

            this.GoalBoard = new System.Windows.Forms.Label();

            this.GoalBoardBox1 = new System.Windows.Forms.TextBox();
            this.GoalBoardBox2 = new System.Windows.Forms.TextBox();
            this.GoalBoardBox3 = new System.Windows.Forms.TextBox();
            this.GoalBoardBox4 = new System.Windows.Forms.TextBox();
            this.GoalBoardBox5 = new System.Windows.Forms.TextBox();
            this.GoalBoardBox6 = new System.Windows.Forms.TextBox();
            this.GoalBoardBox7 = new System.Windows.Forms.TextBox();
            this.GoalBoardBox8 = new System.Windows.Forms.TextBox();
            this.GoalBoardBox9 = new System.Windows.Forms.TextBox();

            this.CurrentStepBoard = new System.Windows.Forms.Label();

            this.CurrentStepBoardBox1 = new System.Windows.Forms.TextBox();
            this.CurrentStepBoardBox2 = new System.Windows.Forms.TextBox();
            this.CurrentStepBoardBox3 = new System.Windows.Forms.TextBox();
            this.CurrentStepBoardBox4 = new System.Windows.Forms.TextBox();
            this.CurrentStepBoardBox5 = new System.Windows.Forms.TextBox();
            this.CurrentStepBoardBox6 = new System.Windows.Forms.TextBox();
            this.CurrentStepBoardBox7 = new System.Windows.Forms.TextBox();
            this.CurrentStepBoardBox8 = new System.Windows.Forms.TextBox();
            this.CurrentStepBoardBox9 = new System.Windows.Forms.TextBox();

            this.PrevButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.SaveToFileButton = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();

            this.panel1 = new System.Windows.Forms.Panel();
            
            this.RandomStartButton = new System.Windows.Forms.Button();
            this.AStarCheckBox = new System.Windows.Forms.CheckBox();
            this.RBFSCheckBox  = new System.Windows.Forms.CheckBox();
            this.SolveButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();

            this.AlgorithmChoiceLabel = new System.Windows.Forms.Label();
            this.StepsTakenLabel = new System.Windows.Forms.Label();
            this.IterationsLabel = new System.Windows.Forms.Label();
            this.NodesInMemoryLabel = new System.Windows.Forms.Label();

            this.StepsTakenTextBox = new System.Windows.Forms.TextBox();
            this.IterationsTextBox = new System.Windows.Forms.TextBox();
            this.NodesInMemoryTextBox = new System.Windows.Forms.TextBox();

            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartBoard
            // 
            this.StartBoard.Location = new System.Drawing.Point(12, 9);
            this.StartBoard.Name = "StartBoard";
            this.StartBoard.Size = new System.Drawing.Size(100, 23);
            this.StartBoard.Text = "Стартова матриця";
            // 
            // textBox1
            // 
            this.StartBoardBox1.Location = new System.Drawing.Point(24, 35);
            this.StartBoardBox1.MaxLength = 1;
            this.StartBoardBox1.Name = "StartBoardBox1";
            this.StartBoardBox1.Size = new System.Drawing.Size(20, 20);
            this.StartBoardBox1.TabIndex = 1;
            // 
            // StartBoardBox2
            // 
            this.StartBoardBox2.Location = new System.Drawing.Point(50, 35);
            this.StartBoardBox2.MaxLength = 1;
            this.StartBoardBox2.Name = "StartBoardBox2";
            this.StartBoardBox2.Size = new System.Drawing.Size(20, 20);
            this.StartBoardBox2.TabIndex = 2;
            // 
            // StartBoardBox3
            // 
            this.StartBoardBox3.Location = new System.Drawing.Point(76, 35);
            this.StartBoardBox3.MaxLength = 1;
            this.StartBoardBox3.Name = "StartBoardBox3";
            this.StartBoardBox3.Size = new System.Drawing.Size(20, 20);
            this.StartBoardBox3.TabIndex = 3;
            // 
            // StartBoardBox4
            // 
            this.StartBoardBox4.Location = new System.Drawing.Point(24, 61);
            this.StartBoardBox4.MaxLength = 1;
            this.StartBoardBox4.Name = "StartBoardBox4";
            this.StartBoardBox4.Size = new System.Drawing.Size(20, 20);
            this.StartBoardBox4.TabIndex = 4;
            // 
            // StartBoardBox5
            // 
            this.StartBoardBox5.Location = new System.Drawing.Point(50, 61);
            this.StartBoardBox5.MaxLength = 1;
            this.StartBoardBox5.Name = "StartBoardBox5";
            this.StartBoardBox5.Size = new System.Drawing.Size(20, 20);
            this.StartBoardBox5.TabIndex = 5;
            // 
            // StartBoardBox6
            // 
            this.StartBoardBox6.Location = new System.Drawing.Point(76, 61);
            this.StartBoardBox6.MaxLength = 1;
            this.StartBoardBox6.Name = "StartBoardBox6";
            this.StartBoardBox6.Size = new System.Drawing.Size(20, 20);
            this.StartBoardBox6.TabIndex = 6;
            // 
            // StartBoardBox7
            // 
            this.StartBoardBox7.Location = new System.Drawing.Point(24, 87);
            this.StartBoardBox7.MaxLength = 1;
            this.StartBoardBox7.Name = "StartBoardBox7";
            this.StartBoardBox7.Size = new System.Drawing.Size(20, 20);
            this.StartBoardBox7.TabIndex = 7;
            // 
            // StartBoardBox8
            // 
            this.StartBoardBox8.Location = new System.Drawing.Point(50, 87);
            this.StartBoardBox8.MaxLength = 1;
            this.StartBoardBox8.Name = "StartBoardBox8";
            this.StartBoardBox8.Size = new System.Drawing.Size(20, 20);
            this.StartBoardBox8.TabIndex = 8;
            // 
            // StartBoardBox9
            // 
            this.StartBoardBox9.Location = new System.Drawing.Point(76, 87);
            this.StartBoardBox9.MaxLength = 1;
            this.StartBoardBox9.Name = "StartBoardBox9";
            this.StartBoardBox9.Size = new System.Drawing.Size(20, 20);
            this.StartBoardBox9.TabIndex = 9;
            // 
            // GoalBoard
            // 
            this.GoalBoard.Location = new System.Drawing.Point(150, 9);
            this.GoalBoard.Name = "GoalBoard";
            this.GoalBoard.Size = new System.Drawing.Size(100, 23);
            this.GoalBoard.Text = "Цільова матриця";
            // 
            // GoalBoardBox10
            // 
            this.GoalBoardBox1.Location = new System.Drawing.Point(160, 35);
            this.GoalBoardBox1.MaxLength = 1;
            this.GoalBoardBox1.ReadOnly = true;
            this.GoalBoardBox1.Text = "1";
            this.GoalBoardBox1.Name = "GoalBoardBox1";
            this.GoalBoardBox1.Size = new System.Drawing.Size(20, 20);
            this.GoalBoardBox1.TabIndex = 10;
            // 
            // GoalBoardBox2
            // 
            this.GoalBoardBox2.Location = new System.Drawing.Point(186, 35);
            this.GoalBoardBox2.MaxLength = 1;
            this.GoalBoardBox2.ReadOnly = true;
            this.GoalBoardBox2.Text = "2";
            this.GoalBoardBox2.Name = "GoalBoardBox2";
            this.GoalBoardBox2.Size = new System.Drawing.Size(20, 20);
            this.GoalBoardBox2.TabIndex = 11;
            // 
            // GoalBoardBox3
            // 
            this.GoalBoardBox3.Location = new System.Drawing.Point(212, 35);
            this.GoalBoardBox3.MaxLength = 1;
            this.GoalBoardBox3.ReadOnly = true;
            this.GoalBoardBox3.Text = "3";
            this.GoalBoardBox3.Name = "GoalBoardBox3";
            this.GoalBoardBox3.Size = new System.Drawing.Size(20, 20);
            this.GoalBoardBox3.TabIndex = 12;
            // 
            // GoalBoardBox4
            // 
            this.GoalBoardBox4.Location = new System.Drawing.Point(160, 61);
            this.GoalBoardBox4.MaxLength = 1;
            this.GoalBoardBox4.ReadOnly = true;
            this.GoalBoardBox4.Text = "4";
            this.GoalBoardBox4.Name = "GoalBoardBox4";
            this.GoalBoardBox4.Size = new System.Drawing.Size(20, 20);
            this.GoalBoardBox4.TabIndex = 13;
            // 
            // GoalBoardBox5
            // 
            this.GoalBoardBox5.Location = new System.Drawing.Point(186, 61);
            this.GoalBoardBox5.MaxLength = 1;
            this.GoalBoardBox5.ReadOnly = true;
            this.GoalBoardBox5.Text = "5";
            this.GoalBoardBox5.Name = "GoalBoardBox5";
            this.GoalBoardBox5.Size = new System.Drawing.Size(20, 20);
            this.GoalBoardBox5.TabIndex = 14;
            // 
            // GoalBoardBox6
            // 
            this.GoalBoardBox6.Location = new System.Drawing.Point(212, 61);
            this.GoalBoardBox6.MaxLength = 1;
            this.GoalBoardBox6.ReadOnly = true;
            this.GoalBoardBox6.Text = "6";
            this.GoalBoardBox6.Name = "GoalBoardBox6";
            this.GoalBoardBox6.Size = new System.Drawing.Size(20, 20);
            this.GoalBoardBox6.TabIndex = 15;
            // 
            // GoalBoardBox7
            // 
            this.GoalBoardBox7.Location = new System.Drawing.Point(160, 87);
            this.GoalBoardBox7.MaxLength = 1;
            this.GoalBoardBox7.ReadOnly = true;
            this.GoalBoardBox7.Text = "7";
            this.GoalBoardBox7.Name = "GoalBoardBox7";
            this.GoalBoardBox7.Size = new System.Drawing.Size(20, 20);
            this.GoalBoardBox7.TabIndex = 16;
            // 
            // GoalBoardBox8
            // 
            this.GoalBoardBox8.Location = new System.Drawing.Point(186, 87);
            this.GoalBoardBox8.MaxLength = 1;
            this.GoalBoardBox8.ReadOnly = true;
            this.GoalBoardBox8.Text = "8";
            this.GoalBoardBox8.Name = "GoalBoardBox8";
            this.GoalBoardBox8.Size = new System.Drawing.Size(20, 20);
            this.GoalBoardBox8.TabIndex = 17;
            // 
            // GoalBoardBox9
            // 
            this.GoalBoardBox9.Location = new System.Drawing.Point(212, 87);
            this.GoalBoardBox9.MaxLength = 1;
            this.GoalBoardBox9.ReadOnly = true;
            this.GoalBoardBox9.Text = "0";
            this.GoalBoardBox9.Name = "GoalBoardBox9";
            this.GoalBoardBox9.Size = new System.Drawing.Size(20, 20);
            this.GoalBoardBox9.TabIndex = 18;
            // 
            // CurrentStepBoard
            // 
            this.CurrentStepBoard.Location = new System.Drawing.Point(81, 126);
            this.CurrentStepBoard.Name = "CurrentStepBoard";
            this.CurrentStepBoard.Size = new System.Drawing.Size(100, 23);
            this.CurrentStepBoard.Text = "Кроки";
            // 
            // CurrentStepBoardBox1
            // 
            this.CurrentStepBoardBox1.Location = new System.Drawing.Point(91, 152);
            this.CurrentStepBoardBox1.MaxLength = 1;
            this.CurrentStepBoardBox1.Name = "CurrentStepBoardBox1";
            this.CurrentStepBoardBox1.ReadOnly = true;
            this.CurrentStepBoardBox1.TabStop = false;
            this.CurrentStepBoardBox1.Size = new System.Drawing.Size(20, 20);
            // 
            // CurrentStepBoardBox2
            // 
            this.CurrentStepBoardBox2.Location = new System.Drawing.Point(117, 152);
            this.CurrentStepBoardBox2.MaxLength = 1;
            this.CurrentStepBoardBox2.Name = "CurrentStepBoardBox2";
            this.CurrentStepBoardBox2.ReadOnly = true;
            this.CurrentStepBoardBox2.TabStop = false;
            this.CurrentStepBoardBox2.Size = new System.Drawing.Size(20, 20);
            // 
            // CurrentStepBoardBox3
            // 
            this.CurrentStepBoardBox3.Location = new System.Drawing.Point(143, 152);
            this.CurrentStepBoardBox3.MaxLength = 1;
            this.CurrentStepBoardBox3.Name = "CurrentStepBoardBox3";
            this.CurrentStepBoardBox3.ReadOnly = true;
            this.CurrentStepBoardBox3.TabStop = false;
            this.CurrentStepBoardBox3.Size = new System.Drawing.Size(20, 20);
            // 
            // CurrentStepBoardBox4
            // 
            this.CurrentStepBoardBox4.Location = new System.Drawing.Point(91, 178);
            this.CurrentStepBoardBox4.MaxLength = 1;
            this.CurrentStepBoardBox4.Name = "CurrentStepBoardBox4";
            this.CurrentStepBoardBox4.ReadOnly = true;
            this.CurrentStepBoardBox4.TabStop = false;
            this.CurrentStepBoardBox4.Size = new System.Drawing.Size(20, 20);
            // 
            // CurrentStepBoardBox5
            // 
            this.CurrentStepBoardBox5.Location = new System.Drawing.Point(117, 178);
            this.CurrentStepBoardBox5.MaxLength = 1;
            this.CurrentStepBoardBox5.Name = "CurrentStepBoardBox5";
            this.CurrentStepBoardBox5.ReadOnly = true;
            this.CurrentStepBoardBox5.TabStop = false;
            this.CurrentStepBoardBox5.Size = new System.Drawing.Size(20, 20);
            // 
            // CurrentStepBoardBox6
            // 
            this.CurrentStepBoardBox6.Location = new System.Drawing.Point(143, 178);
            this.CurrentStepBoardBox6.MaxLength = 1;
            this.CurrentStepBoardBox6.Name = "CurrentStepBoardBox6";
            this.CurrentStepBoardBox6.ReadOnly = true;
            this.CurrentStepBoardBox6.TabStop = false;
            this.CurrentStepBoardBox6.Size = new System.Drawing.Size(20, 20);
            // 
            // CurrentStepBoardBox7
            // 
            this.CurrentStepBoardBox7.Location = new System.Drawing.Point(91, 204);
            this.CurrentStepBoardBox7.MaxLength = 1;
            this.CurrentStepBoardBox7.Name = "CurrentStepBoardBox7";
            this.CurrentStepBoardBox7.ReadOnly = true;
            this.CurrentStepBoardBox7.TabStop = false;
            this.CurrentStepBoardBox7.Size = new System.Drawing.Size(20, 20);
            // 
            // CurrentStepBoardBox8
            // 
            this.CurrentStepBoardBox8.Location = new System.Drawing.Point(117, 204);
            this.CurrentStepBoardBox8.MaxLength = 1;
            this.CurrentStepBoardBox8.Name = "CurrentStepBoardBox8";
            this.CurrentStepBoardBox8.ReadOnly = true;
            this.CurrentStepBoardBox8.TabStop = false;
            this.CurrentStepBoardBox8.Size = new System.Drawing.Size(20, 20);
            // 
            // CurrentStepBoardBox9
            // 
            this.CurrentStepBoardBox9.Location = new System.Drawing.Point(143, 204);
            this.CurrentStepBoardBox9.MaxLength = 1;
            this.CurrentStepBoardBox9.Name = "CurrentStepBoardBox9";
            this.CurrentStepBoardBox9.ReadOnly = true;
            this.CurrentStepBoardBox9.TabStop = false;
            this.CurrentStepBoardBox9.Size = new System.Drawing.Size(20, 20);
            // 
            // PrevButton
            // 
            this.PrevButton.Location = new System.Drawing.Point(80, 230);
            this.PrevButton.Name = "PrevButton";
            this.PrevButton.Size = new System.Drawing.Size(45, 23);
            this.PrevButton.TabIndex = 19;
            this.PrevButton.Text = "Prev";
            this.PrevButton.UseVisualStyleBackColor = true;
            this.PrevButton.Click += new System.EventHandler(PrevStepInPath);
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(130, 230);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(45, 23);
            this.NextButton.TabIndex = 21;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(NextStepInPath);
            // 
            // SaveToFileButton
            // 
            this.SaveToFileButton.Location = new System.Drawing.Point(82, 255);
            this.SaveToFileButton.Name = "SaveButton";
            this.SaveToFileButton.Size = new System.Drawing.Size(90, 23);
            this.SaveToFileButton.TabIndex = 22;
            this.SaveToFileButton.Text = "Зберегти";
            this.SaveToFileButton.UseVisualStyleBackColor = true;
            this.SaveToFileButton.Click += new System.EventHandler(SavePathToFile);
            // 
            // Exit
            // 
            this.Exit.Location = new System.Drawing.Point(120, 270);
            this.Exit.Name = "ExitButton";
            this.Exit.Size = new System.Drawing.Size(20, 20);
            this.Exit.TabIndex = 22;
            this.Exit.Text = "Q";
            this.Exit.ForeColor = Color.White;
            this.Exit.BackColor = Color.Crimson;
            this.Exit.Click += new System.EventHandler(Quit);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AlgorithmChoiceLabel);
            this.panel1.Controls.Add(this.StepsTakenLabel);
            this.panel1.Controls.Add(this.IterationsLabel);
            this.panel1.Controls.Add(this.NodesInMemoryLabel);
            this.panel1.Controls.Add(this.StepsTakenTextBox);
            this.panel1.Controls.Add(this.IterationsTextBox);
            this.panel1.Controls.Add(this.NodesInMemoryTextBox);
            this.panel1.Controls.Add(this.AStarCheckBox);
            this.panel1.Controls.Add(this.RBFSCheckBox);
            this.panel1.Controls.Add(this.SolveButton);
            this.panel1.Controls.Add(this.ResetButton);
            this.panel1.Controls.Add(this.RandomStartButton);
            this.panel1.Controls.Add(this.Exit);
            this.panel1.Location = new System.Drawing.Point(256, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 290);
            // 
            // AlgorithmChoiceLabel
            // 
            this.AlgorithmChoiceLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuPopup;
            this.AlgorithmChoiceLabel.Location = new System.Drawing.Point(15, 9);
            this.AlgorithmChoiceLabel.Name = "AlgorithmChoiceLabel";
            this.AlgorithmChoiceLabel.Size = new System.Drawing.Size(100, 13);
            this.AlgorithmChoiceLabel.TabIndex = 8;
            this.AlgorithmChoiceLabel.Text = "Почати :";
            // 
            // StepsTakenTextBox
            // 
            this.StepsTakenTextBox.AcceptsReturn = true;
            this.StepsTakenTextBox.Location = new System.Drawing.Point(15, 170);
            this.StepsTakenTextBox.Name = "StepsTakenTextBox";
            this.StepsTakenTextBox.ReadOnly = true;
            this.StepsTakenTextBox.TabStop = false;
            this.StepsTakenTextBox.Size = new System.Drawing.Size(100, 20);
            // 
            // IterationsTextBox
            // 
            this.IterationsTextBox.Location = new System.Drawing.Point(15, 206);
            this.IterationsTextBox.Name = "IterationsTextBox";
            this.IterationsTextBox.ReadOnly = true;
            this.IterationsTextBox.TabStop = false;
            this.IterationsTextBox.Size = new System.Drawing.Size(100, 20);
            // 
            // NodesInMemoryTextBox
            // 
            this.NodesInMemoryTextBox.AcceptsReturn = true;
            this.NodesInMemoryTextBox.Location = new System.Drawing.Point(15, 250);
            this.NodesInMemoryTextBox.Name = "NodesInMemoryTextBox";
            this.NodesInMemoryTextBox.ReadOnly = true;
            this.NodesInMemoryTextBox.TabStop = false;
            this.NodesInMemoryTextBox.Size = new System.Drawing.Size(100, 20);
            // 
            // StepsTakenLabel
            // 
            this.StepsTakenLabel.BackColor = System.Drawing.SystemColors.Control;
            this.StepsTakenLabel.Location = new System.Drawing.Point(15, 150);
            this.StepsTakenLabel.Name = "StepsTakenLabel";
            this.StepsTakenLabel.Size = new System.Drawing.Size(100, 18);
            this.StepsTakenLabel.Text = "Кроків зроблено:\r\n";
            this.StepsTakenLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // IterationsLabel
            // 
            this.IterationsLabel.Location = new System.Drawing.Point(15, 190);
            this.IterationsLabel.Name = "IterationsLabel";
            this.IterationsLabel.Size = new System.Drawing.Size(100, 16);
            this.IterationsLabel.Text = "Ітерації:";
            // 
            // NodesInMemoryLabel
            // 
            this.NodesInMemoryLabel.Location = new System.Drawing.Point(15, 230);
            this.NodesInMemoryLabel.Name = "NodesInMemoryLabel";
            this.NodesInMemoryLabel.Size = new System.Drawing.Size(100, 18);
            this.NodesInMemoryLabel.Text = "Вершин в пам'яті:";
            // 
            // RandomStartButton
            //
            this.RandomStartButton.Location = new System.Drawing.Point(15, 25);
            this.RandomStartButton.Name = "RandomStartButton";
            this.RandomStartButton.Size = new System.Drawing.Size(122, 22);
            this.RandomStartButton.TabIndex = 22;
            this.RandomStartButton.Text = "Випадкова матриця";
            this.RandomStartButton.UseVisualStyleBackColor = true;
            this.RandomStartButton.Click += new System.EventHandler(GetRandomStart);
            // 
            // StartAStarButton
            // 
            this.AStarCheckBox.Location = new System.Drawing.Point(15, 50);
            this.AStarCheckBox.Name = "AStarCheckBox";
            this.AStarCheckBox.Size = new System.Drawing.Size(122, 20);
            this.AStarCheckBox.TabIndex = 23;
            this.AStarCheckBox.Text = "A*";
            this.AStarCheckBox.UseVisualStyleBackColor = true;
            this.AStarCheckBox.Click += new System.EventHandler(NeutralizeAStar);
            // 
            // RBFSCheckBox
            // 
            this.RBFSCheckBox.Location = new System.Drawing.Point(15, 75);
            this.RBFSCheckBox.Name = "RBFSCheckBox";
            this.RBFSCheckBox.Size = new System.Drawing.Size(122, 22);
            this.RBFSCheckBox.TabIndex = 24;
            this.RBFSCheckBox.Text = "RBFS";
            this.RBFSCheckBox.UseVisualStyleBackColor = true;
            this.RBFSCheckBox.Click += new System.EventHandler(NeutralizeRBFS);
            // 
            // ResetButton
            // 
            this.SolveButton.Location = new System.Drawing.Point(15, 100);
            this.SolveButton.Name = "SearchButton";
            this.SolveButton.Size = new System.Drawing.Size(122, 22);
            this.SolveButton.TabIndex = 25;
            this.SolveButton.Text = "Шукати";
            this.SolveButton.UseVisualStyleBackColor = true;
            this.SolveButton.Click += new System.EventHandler(StartSearch);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(15, 125);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(122, 22);
            this.ResetButton.TabIndex = 25;
            this.ResetButton.Text = "Зтерти";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(Reset);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(405, 300);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.PrevButton);
            this.Controls.Add(this.SaveToFileButton);
            this.Controls.Add(this.CurrentStepBoardBox9);
            this.Controls.Add(this.CurrentStepBoardBox8);
            this.Controls.Add(this.CurrentStepBoardBox7);
            this.Controls.Add(this.CurrentStepBoardBox6);
            this.Controls.Add(this.CurrentStepBoardBox5);
            this.Controls.Add(this.CurrentStepBoardBox4);
            this.Controls.Add(this.CurrentStepBoardBox3);
            this.Controls.Add(this.CurrentStepBoardBox2);
            this.Controls.Add(this.CurrentStepBoardBox1);
            this.Controls.Add(this.CurrentStepBoard);
            this.Controls.Add(this.GoalBoardBox9);
            this.Controls.Add(this.GoalBoardBox8);
            this.Controls.Add(this.GoalBoardBox7);
            this.Controls.Add(this.GoalBoardBox6);
            this.Controls.Add(this.GoalBoardBox5);
            this.Controls.Add(this.GoalBoardBox4);
            this.Controls.Add(this.GoalBoardBox3);
            this.Controls.Add(this.GoalBoardBox2);
            this.Controls.Add(this.GoalBoardBox1);
            this.Controls.Add(this.GoalBoard);
            this.Controls.Add(this.StartBoardBox9);
            this.Controls.Add(this.StartBoardBox8);
            this.Controls.Add(this.StartBoardBox7);
            this.Controls.Add(this.StartBoardBox6);
            this.Controls.Add(this.StartBoardBox5);
            this.Controls.Add(this.StartBoardBox4);
            this.Controls.Add(this.StartBoardBox3);
            this.Controls.Add(this.StartBoardBox2);
            this.Controls.Add(this.StartBoardBox1);
            this.Controls.Add(this.StartBoard);
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "UI";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label AlgorithmChoiceLabel;
        private System.Windows.Forms.TextBox StepsTakenTextBox;
        private System.Windows.Forms.TextBox IterationsTextBox;
        private System.Windows.Forms.TextBox NodesInMemoryTextBox;


        private System.Windows.Forms.Label StepsTakenLabel;
        private System.Windows.Forms.Label IterationsLabel;
        private System.Windows.Forms.Label NodesInMemoryLabel;

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button RandomStartButton;
        private System.Windows.Forms.CheckBox AStarCheckBox;
        private System.Windows.Forms.CheckBox RBFSCheckBox;
        private System.Windows.Forms.Button SolveButton;
        private System.Windows.Forms.Button ResetButton;

        private System.Windows.Forms.Button PrevButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button SaveToFileButton;
        private System.Windows.Forms.Button Exit;

        private System.Windows.Forms.TextBox CurrentStepBoardBox1;
        private System.Windows.Forms.TextBox CurrentStepBoardBox2;
        private System.Windows.Forms.TextBox CurrentStepBoardBox3;
        private System.Windows.Forms.TextBox CurrentStepBoardBox4;
        private System.Windows.Forms.TextBox CurrentStepBoardBox5;
        private System.Windows.Forms.TextBox CurrentStepBoardBox6;
        private System.Windows.Forms.TextBox CurrentStepBoardBox7;
        private System.Windows.Forms.TextBox CurrentStepBoardBox8;
        private System.Windows.Forms.TextBox CurrentStepBoardBox9;

        private System.Windows.Forms.Label CurrentStepBoard;

        private System.Windows.Forms.TextBox GoalBoardBox1;
        private System.Windows.Forms.TextBox GoalBoardBox2;
        private System.Windows.Forms.TextBox GoalBoardBox3;
        private System.Windows.Forms.TextBox GoalBoardBox4;
        private System.Windows.Forms.TextBox GoalBoardBox5;
        private System.Windows.Forms.TextBox GoalBoardBox6;
        private System.Windows.Forms.TextBox GoalBoardBox7;
        private System.Windows.Forms.TextBox GoalBoardBox8;
        private System.Windows.Forms.TextBox GoalBoardBox9;

        private System.Windows.Forms.Label GoalBoard;

        private System.Windows.Forms.TextBox StartBoardBox1;
        private System.Windows.Forms.TextBox StartBoardBox2;
        private System.Windows.Forms.TextBox StartBoardBox3;
        private System.Windows.Forms.TextBox StartBoardBox4;
        private System.Windows.Forms.TextBox StartBoardBox5;
        private System.Windows.Forms.TextBox StartBoardBox6;
        private System.Windows.Forms.TextBox StartBoardBox7;
        private System.Windows.Forms.TextBox StartBoardBox8;
        private System.Windows.Forms.TextBox StartBoardBox9;

        private System.Windows.Forms.Label StartBoard;

        #endregion
    }
}
