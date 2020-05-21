using ByteSizeLib;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DriveMounter
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Only mounted volumes have a drive letter. We need to cache the drive letter before
        /// unmounting otherwise that information is lost, which would force us to ask the user
        /// to re-enter the drive letter when remounting the volume.
        /// </summary>
        private readonly DriveLetterCache driveLetterCache = new DriveLetterCache();

        private List<WindowsVolume> volumes;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshVolumes();
        }

        private void volumesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (volumesListView.SelectedIndices.Count == 0)
            {
                mountButton.Enabled = false;
                return;
            }

            WindowsVolume volume = volumes[volumesListView.SelectedIndices[0]];
            if (volume.IsMounted)
            {
                mountButton.Text = "Unmount";
            }
            else
            {
                mountButton.Text = "Mount";
            }

            mountButton.Enabled = true;
        }

        private void mountButton_Click(object sender, EventArgs e)
        {
            if (mountButton.Text == "Mount")
            {
                Mount();
            }
            else if (mountButton.Text == "Unmount")
            {
                Unmount();
            }
            else
            {
                throw new InvalidOperationException("Unknown button state.");
            }
        }

        private void Mount()
        {
            WindowsVolume volume = volumes[volumesListView.SelectedIndices[0]];
            if (volume.IsMounted)
            {
                MessageBox.Show(
                    $"Volume is already mounted to drive {volume.DriveLetter}",
                    "Mount canceled.", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            string lastKnownDriveLetter =
                driveLetterCache.GetDriveLetter(volume) ??
                Prompt.ShowDialog("Enter a drive letter to mount to (e.g. D:):", "What drive letter to use?");

            if (string.IsNullOrEmpty(lastKnownDriveLetter))
            {
                MessageBox.Show(
                    "Could not mount volume: last known drive letter could not be determined.",
                    "Mount failed.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to mount volume?", "Mount volume?", MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes)
            {
                return;
            }

            volume = volume.WithDriveLetter(lastKnownDriveLetter);
            try
            {
                volume.Mount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not mount volume: unexpected error: {ex}",
                    "Mount failed.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            driveLetterCache.DeleteDriveLetter(volume);
            RefreshVolumes();

            MessageBox.Show(
                "Volume was successfully mounted.",
                "Volume mounted.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Unmount()
        {
            WindowsVolume volume = volumes[volumesListView.SelectedIndices[0]];
            if (!volume.IsMounted)
            {
                MessageBox.Show(
                    "Volume is already unmounted.",
                    "Unmount canceled.", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to unmount volume? You will not be able to access it until you remount it.",
                "Unmount volume?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                volume.Unmount();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not unmount volume: unexpected error: {ex}",
                    "Unmount failed.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            driveLetterCache.SaveDriveLetter(volume);
            RefreshVolumes();

            MessageBox.Show(
                "Volume was successfully unmounted.",
                "Volume unmounted.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RefreshVolumes()
        {
            // Store current selection so we can re-apply it after repopulating ListView.
            WindowsVolume selectedVolume = null;
            if (volumesListView.SelectedIndices.Count > 0)
            {
                int idx = volumesListView.SelectedIndices[0];
                selectedVolume = volumes[idx];
            }

            volumes = WindowsVolume.GetAllWindowsVolumes();

            volumesListView.Items.Clear();
            foreach (var volume in volumes)
            {
                string driveLetter = volume.DriveLetter ?? driveLetterCache.GetDriveLetter(volume);

                volumesListView.Items.Add(new ListViewItem(new[]
                {
                    $"{driveLetter ?? ""}",
                    $"{volume.DriveName ?? ""}",
                    $"{volume.IsMounted}",
                    $"{ByteSize.FromBytes(volume.FreeSpace)}",
                    $"{ByteSize.FromBytes(volume.Capacity)}",
                }));
            }

            if (selectedVolume != null)
            {
                int newIdx = volumes.FindIndex(v => v.ID == selectedVolume.ID);
                if (newIdx != -1)
                {
                    volumesListView.SelectedIndices.Clear();
                    volumesListView.SelectedIndices.Add(newIdx);
                }
            }
        }
    }
}
