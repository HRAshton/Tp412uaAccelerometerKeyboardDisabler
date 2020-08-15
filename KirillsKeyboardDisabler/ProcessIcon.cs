using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Sensors;

namespace Tp412uaAccelerometerKeyboardDisabler
{
    /// <summary>
    /// 
    /// </summary>
    class ProcessIcon : IDisposable
    {
        /// <summary>
        /// PKEY_SensorData_CustomValue2.
        /// </summary>
        private const string AccelerometerPropertyId = "{C458F8A7-4AE8-4777-9607-2E9BDD65110A} 162";

        private bool isKeyboardDisabled;
        private LidPositions lastPosition;
        private readonly NotifyIcon notifyIcon;
        private readonly Accelerometer accelerometer;

        private readonly Func<bool, bool> BlockInput;

        public ProcessIcon(Func<bool, bool> blockInput)
        {
            notifyIcon = new NotifyIcon()
            {
                Text = "HRAshton's helper app that can disable keyboard",
                ContextMenuStrip = new ContextMenus().Create(),
                Visible = true,
            };
            notifyIcon.MouseClick += Ni_MouseClick;

            accelerometer = Accelerometer.GetDefault();
            Task.Run(async () =>
            {
                while (true)
                {
                    ProcessState();
                    await Task.Delay(500);
                }
            });

            BlockInput = blockInput;

            SetKeyboardState(isKeyboardDisabled);
        }

        private void ProcessState()
        {
            LidPositions position = (LidPositions)(uint)accelerometer.GetCurrentReading()
                .Properties[AccelerometerPropertyId];
            if (lastPosition == position)
                return;

            lastPosition = position;

            bool shouldInputBeBlocked = lastPosition > LidPositions.Open180deg;
            if (isKeyboardDisabled != shouldInputBeBlocked)
                SetKeyboardState(shouldInputBeBlocked);
        }

        public void Dispose() => notifyIcon.Dispose();

        private void Ni_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ToggleKeyboardState();
        }

        private void ToggleKeyboardState()
        {
            SetKeyboardState(!isKeyboardDisabled);
        }

        private void SetKeyboardState(bool newState)
        {
            isKeyboardDisabled = newState;

            if (newState)
            {
                BlockInput(true);
                notifyIcon.Icon = Properties.Resources.off;
            }
            else
            {
                BlockInput(false);
                notifyIcon.Icon = Properties.Resources.on;
            }
        }
    }
}
