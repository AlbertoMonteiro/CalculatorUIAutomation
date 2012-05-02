using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace ConsoleApplication4
{
    public class WindowsCalculator : IDisposable
    {
        private readonly Process processField;
        private AutomationElement calculator;
        private AutomationElement resultTextBox;

        public WindowsCalculator()
        {
            processField = Process.Start("Calc.exe");

            InitializeCalculatorAutomationElement();
            InitializeResultTextBoxAutomationElement();
        }

        public double Result
        {
            get { return Convert.ToDouble(resultTextBox.GetCurrentPropertyValue(AutomationElement.NameProperty)); }
        }

        #region IDisposable Members

        public void Dispose()
        {
            while (!processField.HasExited)
                processField.CloseMainWindow();
            processField.Dispose();
        }

        #endregion

        private void InitializeCalculatorAutomationElement()
        {
            var count = 0;

            while (calculator == null && count < 50)
            {
                var nameIsCalculadora = new PropertyCondition(AutomationElement.NameProperty, "Calculadora");
                calculator = GetElement(nameIsCalculadora, TreeScope.Children);

                ++count;
                Thread.Sleep(100);
            }

            if (calculator == null)
                throw new InvalidOperationException("Calculator must be running.");
        }

        private void InitializeResultTextBoxAutomationElement()
        {
            resultTextBox = GetElement(new PropertyCondition(AutomationElement.AutomationIdProperty, "158"), TreeScope.Descendants);

            if (resultTextBox == null)
                throw new InvalidOperationException("Could not find result text box.");
        }

        private static AutomationElement GetElement(Condition propertyCondition, TreeScope treeScope)
        {
            return AutomationElement.RootElement.FindFirst(treeScope, propertyCondition);
        }

        public void PressDigit(int digit)
        {
            const int button0Id = 130;
            var oneButton = GetElement(new PropertyCondition(AutomationElement.AutomationIdProperty, (button0Id+digit).ToString()), TreeScope.Descendants);
            var invokePattern = GetInvokePattern(oneButton);

            if (invokePattern != null)
                invokePattern.Invoke();
        }

        private static InvokePattern GetInvokePattern(AutomationElement oneButton)
        {
            var invokePattern = oneButton.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            return invokePattern;
        }

        public bool PressAdd()
        {
            var preesed = false;
            var addButton = GetElement(new PropertyCondition(AutomationElement.AutomationIdProperty, "93"), TreeScope.Descendants);

            var invokePattern = GetInvokePattern(addButton);

            if (invokePattern != null)
            {
                invokePattern.Invoke();
                preesed = true;
            }

            return preesed;
        }

        public bool PressEquals()
        {
            var preesed = false;
            var equalsButton = GetElement(new PropertyCondition(AutomationElement.AutomationIdProperty, "121"), TreeScope.Descendants);

            var invokePattern = GetInvokePattern(equalsButton);

            if (invokePattern != null)
            {
                invokePattern.Invoke();
                preesed = true;
            }

            return preesed;
        }
    }
}