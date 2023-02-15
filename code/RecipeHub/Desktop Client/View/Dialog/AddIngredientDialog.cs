﻿using Desktop_Client.ViewModel.Ingredients;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared_Resources.Model.Ingredients;

namespace Desktop_Client.View.Dialog
{
    /// <summary>
    /// Dialog for Adding an Ingredient.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class AddIngredientDialog : Form
    {
        private readonly AddIngredientsViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddIngredientDialog"/> class.<br />
        /// <br />
        /// Precondition: None<br />
        /// Postcondition: Default values is set for fields.<br />
        /// </summary>
        public AddIngredientDialog()
        {
            InitializeComponent();
            this.viewModel = new AddIngredientsViewModel();
            this.measurementComboBox.DataSource = Enum.GetValues(typeof(MeasurementType));
        }

        private void addIngredientButton_Click(object sender, EventArgs e)
        {
            this.viewModel.AddIngredient(new Shared_Resources.Model.Ingredients.Ingredient(this.nameComboBox.Text,
                int.Parse(this.amountTextBox.Text), (MeasurementType)this.measurementComboBox.SelectedValue!));
            this.Close();
            this.Dispose();
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            this.DialogResult = DialogResult.Cancel;
        }

        private void nameComboBox_TextChanged(object sender, EventArgs e)
        {
            //var suggestions = this.viewModel.GetSuggestions(this.nameComboBox.Text);
            //this.nameComboBox.DataSource = suggestions;
        }

        private void amountTextBox_TextChanged(object sender, EventArgs e)
        {
            bool enteredLetter = false;
            Queue<char> text = new Queue<char>();
            foreach (var ch in this.amountTextBox.Text)
            {
                if (char.IsDigit(ch))
                {
                    text.Enqueue(ch);
                }
                else
                {
                    enteredLetter = true;
                }
            }

            if (enteredLetter)
            {
                StringBuilder sb = new StringBuilder();
                while (text.Count > 0)
                {
                    sb.Append(text.Dequeue());
                }

                this.amountTextBox.Text = sb.ToString();
                this.amountTextBox.SelectionStart = this.amountTextBox.Text.Length;
            }
        }
    }
}