using qmanlib.src.storage.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace qman_native.src.utils
{
    internal class ModuleUtils
    {

        public static string GetModuleIcon(Module module) {
            return GetModuleIcon(module.Type);
        }
        public static string GetModuleIcon(string type) {
            return GetModuleIcon(ModuleTypeExtensions.FromModuleName(type));
        }
        public static string GetModuleIcon(ModuleType type) {
            switch (type)
            {
                case ModuleType.SWN04:
                    return "<svg style=\"\"width:48px;height:48px\"\" viewBox=\"\"0 0 48 48\"\"><path d=\"M13,15h6v7h-6V15z M13,14h6v-4h-6V14z M25,5v22c0,1.657-1.343,3-3,3H10c-1.657,0-3-1.343-3-3V5c0-1.657,1.343-3,3-3h12C23.657,2,25,3.343,25,5z M15,6c0,0.552,0.448,1,1,1s1-0.448,1-1c0-0.552-0.448-1-1-1S15,5.448,15,6z M17,26c0-0.552-0.448-1-1-1s-1,0.448-1,1c0,0.552,0.448,1,1,1S17,26.552,17,26z M20,10c0-0.552-0.448-1-1-1h-6c-0.552,0-1,0.448-1,1v12c0,0.552,0.448,1,1,1h6c0.552,0,1-0.448,1-1V10z\"/><svg/>";
                case ModuleType.DIM04_500U:
                    return "<svg style=\"\"width:24px;height:24px\"\" viewBox=\"\"0 0 24 24\"\"><path d=\"M15,14H9V12h2V5h2v7h2Zm-4,5h2V15H11ZM19,3V21a2,2,0,0,1-2,2H7a2,2,0,0,1-2-2V3A2,2,0,0,1,7,1H17A2,2,0,0,1,19,3ZM17,3H7V21H17Z\"/><svg/>";
                case ModuleType.REL04SA:
                    return "<svg style=\"\"width:24px;height:24px\"\" viewBox=\"\"0 0 24 24\"\"><path d=\"M19.5,7h-7V4h-2V7h-7a2,2,0,0,0-2,2v6a2,2,0,0,0,2,2h7v3h2V17h7a2,2,0,0,0,2-2V9A2,2,0,0,0,19.5,7Zm-16,8V9h9.59l-6,6Zm16,0H9.91l6-6H19.5Z\"/><svg/>";
            }
            return "";
        }
    }
}
