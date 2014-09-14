using System;
using System.IO;
using System.Net;
using System.Threading;
using NLog;
namespace FTP_Health_Check
{
    /// <totdo>
    /// Send Email for now
    /// Write Eventlog Error Entry so Nimsoft can report on it
    /// </totdo>
    class Program
    {
        private static Logger logs = LogManager.GetCurrentClassLogger();                            // Console / Log file logging
        /// <summary>
        /// The _server
        /// </summary>
        private static string[] _server = new string[5]
        {
            "<server>",        // FTP Server Address 29
            "<user>",                   // FTP User Name
            "<password>",                     // FTP Password
            "<path>",                                        // FTP Path
            "<file>"                                  // Test File
        };

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException); // Handles unhandled Exception errors
        
            UploadFile();
            Thread.Sleep(5000);
            DeleteFile();

            
        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        private static void UploadFile()
        {
            logs.Info("Uploading file");
            FileInfo testFile = new FileInfo(_server[4]);
            FtpWebRequest ftpRequest;
            
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(_server[0]) + _server[3] + testFile.Name);
            ftpRequest.Credentials = new NetworkCredential(_server[1], _server[2]);
            ftpRequest.KeepAlive = false;
            ftpRequest.UseBinary = true;
            ftpRequest.EnableSsl = true;
            ftpRequest.Proxy = null;
            ftpRequest.ContentLength = testFile.Length;
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            int intBufferLength = 16 * 1024;
            byte[] fileBuffer = new byte[intBufferLength];

            FileStream fileStream = testFile.OpenRead();
            FtpWebResponse ftpResponse;

            try
            {
                Stream stream = ftpRequest.GetRequestStream();

                int len = 0;
                while ((len = fileStream.Read(fileBuffer, 0, intBufferLength)) != 0)
                {
                    stream.Write(fileBuffer, 0, len);
                }
                stream.Close();
                fileStream.Close();
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                logs.Info("Status code {0}", ftpResponse.StatusCode);
                logs.Info("Status description {0}", ftpResponse.StatusDescription);
                ftpResponse.Close();
            }
            catch (UnauthorizedAccessException ae)                                              // Catches unauthorized access messages
            {
                logs.Fatal(ae.Message);                                                  // Logs to console & files
            }
            catch (WebException wex)                                                            // Catches webexceptions
            {
                logs.Fatal(wex.Message);                                                 // Logs to consle & files
            }
            catch (SystemException se)                                                          // Catches system execptions
            {
                logs.Fatal(se.Message);                                                  // Logs to console & files
            }
            catch (ApplicationException ape)                                                    // Catches application exceptions
            {
                logs.Fatal(ape.Message);                                                 // Logs to console & files
            }            
            catch (Exception ex)                                                                // Catches exceptions
            {
                logs.Fatal(ex.Message);                                                  // Logs to console & files
            }
            finally                                                                             // Finally
            {
                Environment.Exit(0);                                                            // Exits
            }
        }

        private static void DeleteFile()
        {
            logs.Info("Deleting file");
            FileInfo testFile = new FileInfo(_server[4]);
            FtpWebRequest ftpRequest;

            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(_server[0]) + _server[3] + testFile.Name);
            ftpRequest.Credentials = new NetworkCredential(_server[1], _server[2]);
            ftpRequest.KeepAlive = false;
            ftpRequest.UseBinary = true;
            ftpRequest.EnableSsl = true;
            ftpRequest.Proxy = null;
            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

            FtpWebResponse ftpResponse;

            try
            {
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                logs.Info("Status code {0}", ftpResponse.StatusCode);
                logs.Info("Status description {0}", ftpResponse.StatusDescription);
                ftpResponse.Close();
            }
            catch (UnauthorizedAccessException ae)                                              // Catches unauthorized access messages
            {
                logs.Fatal(ae.Message);                                                  // Logs to console & files
            }
            catch (WebException wex)                                                            // Catches webexceptions
            {
                logs.Fatal(wex.Message);                                                 // Logs to consle & files
            }
            catch (SystemException se)                                                          // Catches system execptions
            {
                logs.Fatal(se.Message);                                                  // Logs to console & files
            }
            catch (ApplicationException ape)                                                    // Catches application exceptions
            {
                logs.Fatal(ape.Message);                                                 // Logs to console & files
            }
            catch (Exception ex)                                                                // Catches exceptions
            {
                logs.Fatal(ex.Message);                                                  // Logs to console & files
            }
            finally                                                                             // Finally
            {
                Environment.Exit(0);                                                            // Exits
            }
        }

        /// <summary>
        /// Handles the UnhandledException event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="ue">The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ue)
        {
            try
            {
                Exception ex = (Exception)ue.ExceptionObject;
                logs.Fatal(ex.Message);
            }
            finally
            {
                Environment.Exit(0);
            }
        }
    }
}
