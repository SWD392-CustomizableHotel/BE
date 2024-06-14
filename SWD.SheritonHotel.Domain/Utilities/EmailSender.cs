using Microsoft.Extensions.Configuration;
using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;

namespace SWD.SheritonHotel.Domain.Utilities
{
    public class EmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public bool SendForgotPasswordEmail(string email, string token)
        {
            var subject = "[Sheriton Hotel] Reset password request";
            var baseUrl = _config["Environment"] == "Production"
                ? "https://fe-customizablehotel.vercel.app"
                : "http://localhost:4200";
            var verifyUrl = $"{baseUrl}/reset-password?email={email}&token={token}";
            var message = @"
<!DOCTYPE html>
<html dir='ltr' xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office'>
<head>
  <meta charset='UTF-8'>
  <meta content='width=device-width, initial-scale=1' name='viewport'>
  <meta name='x-apple-disable-message-reformatting'>
  <meta http-equiv='X-UA-Compatible' content='IE=edge'>
  <meta content='telephone=no' name='format-detection'>
  <title></title>
  <!--[if (mso 16)]>
    <style type='text/css'>
      a {text - decoration: none;}
    </style>
  <![endif]-->
  <!--[if gte mso 9]><style>sup {font - size: 100% !important; }</style><![endif]-->
  <!--[if gte mso 9]>
  <xml>
    <o:OfficeDocumentSettings>
      <o:AllowPNG></o:AllowPNG>
      <o:PixelsPerInch>96</o:PixelsPerInch>
    </o:OfficeDocumentSettings>
  </xml>
  <![endif]-->
  <!--[if !mso]><!-- -->
  <link href='https://fonts.googleapis.com/css2?family=Imprima&display=swap' rel='stylesheet'>
  <!--<![endif]-->
  <!--[if mso]>
  <style type='text/css'>
    ul {margin: 0 !important; }
    ol {margin: 0 !important; }
    li {margin - left: 47px !important; }
  </style>
  <![endif]-->
</head>
<body style='width: 100%; height: 100%; padding: 0; margin: 0;'>
  <div dir='ltr' style='background-color: #ffffff;'>
    <!--[if gte mso 9]>
    <v:background xmlns:v='urn:schemas-microsoft-com:vml' fill='t'>
      <v:fill type='tile' color='#ffffff'></v:fill>
    </v:background>
    <![endif]-->
    <table style='width: 100%; border-collapse: collapse; border-spacing: 0px;' cellspacing='0' cellpadding='0'>
      <tbody>
        <tr>
          <td valign='top' style='padding: 0; margin: 0;'>
            <table cellpadding='0' cellspacing='0' align='center' style='width: 100%; table-layout: fixed;'>
              <tbody>
                <tr>
                  <td align='center'>
                    <table bgcolor='#bcb8b1' align='center' cellpadding='0' cellspacing='0' style='background-color: #bcb8b1; width: 600px;'>
                      <tbody>
                        <tr>
                          <td align='left' style='padding: 20px 40px;'>
                            <table cellpadding='0' cellspacing='0' width='100%'>
                              <tbody>
                                <tr>
                                  <td align='center' valign='top' style='font-size: 0;'>
                                    <a target='_blank'>
                                      <img src='https://ehzfwur.stripocdn.email/content/guids/CABINET_97e0cd87aa35382a18fc002af25dbcee8f5b6b25face755a881abd743feb9faa/images/logo_sheriton_1.png' alt='' width='190' style='display: block; font-size: 18px; border: 0; outline: none; text-decoration: none;'>
                                    </a>
                                  </td>
                                </tr>
                              </tbody>
                            </table>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </td>
                </tr>
              </tbody>
            </table>
            <table cellpadding='0' cellspacing='0' align='center' style='width: 100%; table-layout: fixed;'>
              <tbody>
                <tr>
                  <td align='center'>
                    <table bgcolor='#efefef' align='center' cellpadding='0' cellspacing='0' style='background-color: #efefef; border-radius: 20px 20px 0 0; width: 600px;'>
                      <tbody>
                        <tr>
                          <td align='left' style='padding: 40px;'>
                            <table cellpadding='0' cellspacing='0' width='100%'>
                              <tbody>
                                <tr>
                                  <td align='center' valign='top' style='font-size: 0;'>
                                    <a target='_blank' href='" + baseUrl + @"'>
                                      <img src='https://ehzfwur.stripocdn.email/content/guids/CABINET_97e0cd87aa35382a18fc002af25dbcee8f5b6b25face755a881abd743feb9faa/images/logo_sheriton_1.png' alt='' width='190' style='display: block; font-size: 18px; border: 0; outline: none; text-decoration: none;'>
                                    </a>
                                  </td>
                                </tr>
                              </tbody>
                            </table>
                          </td>
                        </tr>
                        <tr>
                          <td align='left' style='padding: 20px 40px;'>
                            <table cellpadding='0' cellspacing='0' width='100%'>
                              <tbody>
                                <tr>
                                  <td align='center' valign='top'>
                                    <table cellpadding='0' cellspacing='0' width='100%' bgcolor='#fafafa' style='background-color: #fafafa; border-radius: 10px; border-collapse: separate;'>
                                      <tbody>
                                        <tr>
                                          <td align='left' style='padding: 20px;'>
                                            <h3 style='margin: 0; font-family: Imprima, Arial, sans-serif; mso-line-height-rule: exactly; letter-spacing: 0;'>Hi, " + email + @"</h3>
                                            <p><br></p>
                                            <p style='font-family: Imprima, Arial, sans-serif; line-height: 150%; letter-spacing: 0;'>You're receiving this message because you recently requested to reset your password for your account.<br><br>If this was indeed your request, please click the button below to set your new password.</p>
                                          </td>
                                        </tr>
                                      </tbody>
                                    </table>
                                  </td>
                                </tr>
                              </tbody>
                            </table>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </td>
                </tr>
              </tbody>
            </table>
            <table cellpadding='0' cellspacing='0' align='center' style='width: 100%; table-layout: fixed;'>
              <tbody>
                <tr>
                  <td align='center'>
                    <table bgcolor='#efefef' align='center' cellpadding='0' cellspacing='0' style='background-color: #efefef; width: 600px;'>
                      <tbody>
                        <tr>
                          <td align='left' style='padding: 30px 40px;'>
                            <table cellpadding='0' cellspacing='0' width='100%'>
                              <tbody>
                                <tr>
                                  <td align='center' valign='top'>
                                    <table cellpadding='0' cellspacing='0' width='100%'>
                                      <tbody>
                                        <tr>
                                          <td align='center' style='font-size: 0;'>
                                            <!--[if mso]><a href='' target='_blank' hidden>
                                              <v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' xmlns:w='urn:schemas-microsoft-com:office:word' esdevVmlButton href='' style='height:56px; v-text-anchor:middle; width:520px' arcsize='50%' stroke='f' fillcolor='#7630f3'>
                                                <w:anchorlock></w:anchorlock>
                                                <center style='color:#ffffff; font-family:Imprima, Arial, sans-serif; font-size:22px; font-weight:700; line-height:22px; mso-text-raise:1px'>Reset my password</center>
                                              </v:roundrect></a>
                                            <![endif]-->
                                            <!--[if !mso]><!-- --><span style='display: block; background: #7630f3; border-radius: 30px; border-color: #2cb543; border-width: 0px; padding: 15px 20px;'>
                                              <a href='" + verifyUrl + @"' target='_blank' style='display: block; background: #7630f3; border-radius: 30px; font-family: Imprima, Arial, sans-serif; font-size: 22px; font-weight: bold; line-height: 120%; text-align: center; text-decoration: none; color: #ffffff;'>
                                                Reset my password
                                              </a>
                                            </span>
                                            <!--<![endif]-->
                                          </td>
                                        </tr>
                                      </tbody>
                                    </table>
                                  </td>
                                </tr>
                              </tbody>
                            </table>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </td>
                </tr>
              </tbody>
            </table>
            <table cellpadding='0' cellspacing='0' align='center' style='width: 100%; table-layout: fixed;'>
              <tbody>
                <tr>
                  <td align='center'>
                    <table bgcolor='#efefef' align='center' cellpadding='0' cellspacing='0' style='background-color: #efefef; width: 600px; border-radius: 0 0 20px 20px;'>
                      <tbody>
                        <tr>
                          <td align='left' style='padding: 20px 40px;'>
                            <table cellpadding='0' cellspacing='0' width='100%'>
                              <tbody>
                                <tr>
                                  <td align='center' valign='top'>
                                    <p style='font-family: Imprima, Arial, sans-serif; line-height: 150%; letter-spacing: 0;'>If you didn't request to reset your password, please ignore this email or reply to let us know. This password reset link is valid for the next 24 hours.<br><br>Thanks,<br>Sheriton Hotel<br><br></p>
                                    <p style='font-family: Imprima, Arial, sans-serif; line-height: 150%; letter-spacing: 0; margin: 0;'>P.S. If the button above doesn’t work, please copy and paste the link into your web browser.</p>
                                    <p style='font-family: Imprima, Arial, sans-serif; line-height: 150%; letter-spacing: 0; margin: 0;'><a href='{verifyUrl}' style='color: #7630f3;' target='_blank'>" + verifyUrl + @"</a></p>
                                  </td>
                                </tr>
                              </tbody>
                            </table>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</body>
</html>";

            return SendEmail(email, subject, message);
        }

        private bool SendEmail(string email, string subject, string body)
        {
            try
            {
                var senderEmail = _config["SMTP:Username"];
                var senderPassword = _config["SMTP:Password"];
                var smtpHost = _config["SMTP:Host"];
                var smtpPort = int.Parse(_config["SMTP:Port"]);

                var smtpClient = new SmtpClient(smtpHost)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception _)
            {
                return false;
            }
        }
    }
}
