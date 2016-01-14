﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlatRedBall.Glue.SaveClasses;
using GlueView.Facades;
using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.StateInterpolation;

namespace GlueViewOfficialPlugins.States
{
    public partial class StateSaveControl : UserControl
    {
        #region Fields

        List<Control> mCategoryList = new List<Control>();

        IElement mCurrentElement;

        #endregion

        #region Properties

        public IEnumerable<StateCategoryControl> StateCategoryControls
        {
            get
            {

                foreach (StateCategoryControl control in this.StatePanel.Controls)
                {
                    if (control is StateCategoryControl)
                    {
                        yield return control as StateCategoryControl;
                    }
                }
            }
        }

        bool SuppressSets
        {
            get;
            set;
        }


        public IElement CurrentElement
        {
            set
            {

                bool isSame = false;




                if (mCurrentElement != null && value != null && mCurrentElement.Name == value.Name)
                {
                    isSame = true;
                }

                List<StateCategoryValues> oldStates = new List<StateCategoryValues>();

                if (isSame)
                {
                    foreach (StateCategoryControl control in StateCategoryControls)
                    {
                        oldStates.Add(control.StateCategoryValues);
                    }

                }

                mCurrentElement = value;

                Clear();

                IElement element = mCurrentElement;

                while (element != null)
                {

                    if (value != null)
                    {
                        AddComboBoxIfStatesExist("Uncategorized", element.States, "Uncategorized");

                        foreach (StateSaveCategory category in element.StateCategoryList)
                        {
                            AddComboBoxIfStatesExist(category);
                        }
                    }

                    element = ObjectFinder.Self.GetIElement(element.BaseElement);
                }

                if (oldStates != null && ControlCount == oldStates.Count)
                {
                    SuppressSets = true;
                    for (int i = 0; i < oldStates.Count; i++)
                    {
                        StateCategoryValues scv = oldStates[i];

                        StateCategoryControl control = ControlAtIndex(i);

                        control.StateCategoryValues  = scv;


                    }
                    SuppressSets = false;

                    RefreshStates(null, null);

                }

            }
        }

        int ControlCount
        {
            get
            {
                int count = 0;
                foreach (StateCategoryControl control in StateCategoryControls)
                {
                    count++;
                }
                return count;
            }
        }

        #endregion

        private void SetTextIfContainsState(ComboBox comboBox, string stateName)
        {

        }

        StateCategoryControl ControlAtIndex(int index)
        {
            int indexAt= 0;

            foreach (StateCategoryControl control in StateCategoryControls)
            {
                if (indexAt == index)
                {
                    return control;
                }
                indexAt++;
            }
            return null;
        }

        public StateSaveControl()
        {
            InitializeComponent();
        }

        private void AddComboBoxIfStatesExist(StateSaveCategory category)
        {
            AddComboBoxIfStatesExist(category.Name, category.States, category.Name);
        }

        private void AddComboBoxIfStatesExist(string name, List<StateSave> states, string categoryName)
        {
            if (states.Count != 0)
            {
                StatePanel.SuspendLayout();

                StateCategoryControl scc = new StateCategoryControl();
                this.StatePanel.Controls.Add(scc);
                scc.Initialize(name, states);
                StatePanel.ResumeLayout();
                StatePanel.PerformLayout();

                scc.ItemSelect += new EventHandler(RefreshStates);
            }
        }

        void RefreshStates(object sender, EventArgs e)
        {

            GlueViewCommands.Self.ElementCommands.ShowState("");

            foreach (StateCategoryControl scc in StateCategoryControls)
            {
                scc.ShowStateFromComboBox();
            }
        }

        void Clear()
        {
            while(StatePanel.Controls.Count != 0)
            {
                StatePanel.Controls.RemoveAt(0);
            }
            mCategoryList.Clear();
        }


        public void ApplyInterpolateToState(object firstStateUncasted, object secondStateUncasted, float time, InterpolationType interpolationType, Easing easing)
        {
            // We gotta find the type that this is:
            string categoryName = "Uncategorized";

            StateSave firstState = (StateSave)firstStateUncasted;
            StateSave secondState = (StateSave)secondStateUncasted;
            
            
            Type type = firstState.GetType();

            categoryName = type.Name;

            foreach (var uncastedControl in this.StatePanel.Controls)
            {
                if (uncastedControl is StateCategoryControl)
                {
                    StateCategoryControl control = uncastedControl as StateCategoryControl;

                    if (control.ContainsState(firstState) && control.ContainsState(secondState))
                    {
                        control.ApplyInterpolateToState(firstState, secondState, time, interpolationType, easing);
                        break;
                    }
                }
            }

        }
    }
}
