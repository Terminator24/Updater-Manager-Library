﻿namespace UpdaterManagerLibrary
{
    internal static class UpdateUtilities
    {
        #region GENERAL
        public static string UpdaterArguments { get { return ("/update \"{0}\" \"{1}\" \"{2}\""); } }
        #endregion

        #region TIMEOUT
        public static int DefaultTimeout { get { return 2000; } }

        public static int LongTimeout { get { return 3500; } }
        #endregion

        #region LABELINFO
        public static string UpdateInformation { get { return ("Sto scaricando l'aggiornamento..."); } }

        public static string DownloadCompletedInformation { get { return ("Scaricamento completato. Avvio installazione."); } }
        #endregion
    }
}