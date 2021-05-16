using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace DriveMounter
{
    public class WindowsVolume
    {
        public WindowsVolume(string driveLetter, string driveName, string id, long freeSpace, long capacity)
        {
            DriveLetter = driveLetter;
            DriveName = driveName;
            ID = id ?? throw new ArgumentNullException(nameof(id));
            FreeSpace = freeSpace;
            Capacity = capacity;

            EnsureValidDriveLetter(driveLetter);
            EnsureValidID(id);
        }

        public string DriveLetter { get; }

        public string DriveName { get; }

        public string ID { get; }

        public long FreeSpace { get; }

        public long Capacity { get; }

        public bool IsMounted => DriveLetter != null;

        public WindowsVolume WithDriveLetter(string driveLetter) =>
            new WindowsVolume(driveLetter, DriveName, ID, FreeSpace, Capacity);

        public void Mount()
        {
            if (DriveLetter == null)
            {
                throw new InvalidOperationException("Cannot mount volume without a drive letter.");
            }

            RunVolumeMounterCmd($"{DriveLetter} {ID}");
        }

        public void Unmount()
        {
            if (DriveLetter == null)
            {
                throw new InvalidOperationException("Cannot unmount volume without a drive letter.");
            }

            RunVolumeMounterCmd($"{DriveLetter} /P");
        }

        public override string ToString() => $"{nameof(WindowsVolume)} {{ " +
            $"{nameof(DriveLetter)} = {DriveLetter ?? "<null>"}, " +
            $"{nameof(DriveName)} = {DriveName ?? "<null>"}, " +
            $"{nameof(ID)} = {ID}, " +
            $"{nameof(FreeSpace)} = {FreeSpace}, " +
            $"{nameof(Capacity)} = {Capacity}" +
            $" }}";

        public static List<WindowsVolume> GetWindowsVolumes()
        {
            var ms = new ManagementObjectSearcher("Select * from Win32_Volume");
            var moc = ms.Get();

            return moc
                .Cast<ManagementObject>()
                .Select(mo => ToWindowsVolume(mo.Properties))
                .ToList();
        }

        private static WindowsVolume ToWindowsVolume(PropertyDataCollection driveProperties)
        {
            string driveLetter = driveProperties["DriveLetter"].Value?.ToString();
            string driveName = driveProperties["Label"].Value?.ToString();
            string volumeID = driveProperties["DeviceID"].Value?.ToString();
            string freeSpaceString = driveProperties["FreeSpace"].Value?.ToString();
            string capacityString = driveProperties["Capacity"].Value?.ToString();

            long freeSpace = 0;
            long capacity = 0;

            if (freeSpaceString != null)
            {
                freeSpace = long.Parse(freeSpaceString);
            }
            if (capacityString != null)
            {
                capacity = long.Parse(capacityString);
            }

            return new WindowsVolume(driveLetter, driveName, volumeID, freeSpace, capacity);
        }

        private static void RunVolumeMounterCmd(string args)
        {
            Process p;
            try
            {
                p = Process.Start(new ProcessStartInfo("mountvol", args)
                {
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = false
                });
            }
            catch (SystemException ex)
            {
                throw new VolumeMountException("Could not run volume mounter cmd.", ex);
            }

            try
            {
                bool exited = p.WaitForExit(milliseconds: 5000);
                if (!exited)
                {
                    throw new VolumeMountException("Volume mounter cmd did not finish in time.");
                }
            }
            catch (SystemException ex)
            {
                throw new VolumeMountException("Could not check status of volume mounter cmd.", ex);
            }

            if (p.ExitCode != 0)
            {
                throw new VolumeMountException($"Volume mounter cmd return non-successful exit code: {p.ExitCode}");
            }
        }

        private static void EnsureValidDriveLetter(string driveLetter)
        {
            if (driveLetter == null)
            {
                return;
            }

            // Must be a letter followed by a colon.
            if (driveLetter.Length != 2 || !char.IsLetter(driveLetter[0]) || driveLetter[1] != ':')
            {
                throw new ArgumentException("Invalid drive letter.", nameof(driveLetter));
            }
        }

        private static void EnsureValidID(string id)
        {
            const string prefix = @"\\?\Volume";
            const string suffix = @"\";

            if (id.Length != (Guid.Empty.ToString("B").Length + prefix.Length + suffix.Length))
            {
                throw new ArgumentException("Invalid volume ID length.", nameof(id));
            }

            if (!id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ||
                !id.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid volume ID format.", nameof(id));
            }

            string guid = id.Substring(prefix.Length, id.Length - prefix.Length - suffix.Length);
            if (!Guid.TryParseExact(guid, "B", out _))
            {
                throw new ArgumentException("Invalid volume ID GUID.", nameof(id));
            }
        }
    }
}
