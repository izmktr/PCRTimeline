using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PCRTimeline
{
    public partial class StatusForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        class Box
        {
            public Label label;
            public TextBox textbox;
        }

        List<Box> boxlist = new List<Box>();

        string[] paramatorstring ={
            "Level",
            "PAtk",
            "PDef",
            "MaxHp",
            "Avoid",
            "Accuracy",
            "HPAuto",
            "TPAuto",
            "HPDrain",
            "HealUp",
            "TPUp",
            "TPReduction",
            "Move",
            "MAtk",
            "MDef",
            "PCri",
            "MCri",
        };

        public StatusForm()
        {
            InitializeComponent();
        }

        private void StausForm_Load(object sender, EventArgs e)
        {
            const int top = 40;
            const int height = 30;

            int n = 0;
            foreach (var text in paramatorstring)
            {
                var box = new Box();

                box.label = new Label();
                box.label.Location = new System.Drawing.Point(20, top + n * height);
                box.label.Text = text;

                box.textbox = new TextBox();
                box.textbox.Location = new System.Drawing.Point(160, top + n * height);
                box.textbox.Name = "text";
                box.textbox.Text = text;
                box.textbox.TabIndex = n + 1;
                box.textbox.Size = new System.Drawing.Size(44, 22);


                this.Controls.Add(box.label);
                this.Controls.Add(box.textbox);
                boxlist.Add(box);

                n++;
            }
        }
    }
}
