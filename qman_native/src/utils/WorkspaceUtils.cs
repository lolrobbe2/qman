using qmanlib.src.storage.models;
using src;
using System;
using System.Collections.Generic;
using System.Text;

namespace qman_native.src.utils
{
    internal class WorkspaceUtils
    {
        private static bool _loading = false;
        public static async Task OpenWorkspace(){
            FilePickerFileType fileType = new FilePickerFileType(
              new Dictionary<DevicePlatform, IEnumerable<string>>
              {
                { DevicePlatform.WinUI, new[] { ".qdb" } },
                { DevicePlatform.MacCatalyst, new[] { ".qdb" } },
                { DevicePlatform.Android, new[] { ".qdb" } },
                { DevicePlatform.iOS, new[] { ".qdb" } }
              });
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = fileType,
                PickerTitle = "Select a .qdb workspace"
            });
            if (result is not null)
            {
                WorkspaceManager.Open(result.FullPath);
            }
        }

        public static Place GetPlace(Module module){
            return WorkspaceManager.GetPlaces().Where(place => place.ID == module.LocationId).First();
        }
        public static Place GetPlaceChildren(Place place)
        {
            return WorkspaceManager.GetPlaces().Where(child => child.ParentID == place.ID).OrderBy(child => child.Name).First();

        }
        public static IList<Module> GetModules(Place place){
            return WorkspaceManager.GetModules().Where(mod => mod.LocationId == place.ID).ToList();
        }
    }
}
