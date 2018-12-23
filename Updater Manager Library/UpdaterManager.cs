﻿using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace UpdaterManagerLibrary
{
    public static class UpdaterManager
    {
        public static bool CheckForUpdates(Version currentVersion, string downloadUrl, bool verboseNotify)
        {
            bool operationSuccess = false;

            try
            {
                int connectionTimeout = ((!verboseNotify) ? UpdateUtilities.DefaultTimeout : UpdateUtilities.LongTimeout);

                using (WebClientTimeout webClientTimeout = new WebClientTimeout(connectionTimeout))
                using (StreamReader streamReader = new StreamReader(webClientTimeout.OpenRead(new Uri(downloadUrl))))
                {
                    Versioning versioning = ((Versioning)new XmlSerializer(typeof(Versioning)).Deserialize(streamReader));

                    if (Version.TryParse(versioning.LatestVersion, out Version latestVersion))
                    {
                        if (currentVersion < latestVersion)
                        {
                            using (UpdateForm updateForm = new UpdateForm(versioning.VersionHistory))
                            {
                                if (updateForm.ShowDialog() == DialogResult.OK)
                                {
                                    using (DownloadForm downloadForm = new DownloadForm(versioning.DownloadUrl, versioning.Sha256))
                                    {
                                        if (downloadForm.ShowDialog() == DialogResult.OK)
                                        {
                                            operationSuccess = true;
                                        }
                                    }
                                }
                            }
                        }
                        else if (verboseNotify)
                        {
                            string text = "Nessun aggiornamento trovato.";

                            MessageBox.Show(text, "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                if ((verboseNotify) || (!(exception is WebException)))
                {
                    MessageBox.Show(exception.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return operationSuccess;
        }
    }
}
