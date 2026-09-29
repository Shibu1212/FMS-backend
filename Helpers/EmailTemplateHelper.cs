using System.Net;

namespace FormManagementSystem.Helpers;

public static class EmailTemplateHelper
{
    public static string GetRegistrationApprovalEmail(
        string name,
        string email,
        string temporaryPassword,
        string role,
        string loginUrl)
    {
        var safeName = WebUtility.HtmlEncode(name);
        var safeEmail = WebUtility.HtmlEncode(email);
        var safePassword = WebUtility.HtmlEncode(temporaryPassword);
        var safeRole = WebUtility.HtmlEncode(role);
        var safeLoginUrl = WebUtility.HtmlEncode(loginUrl);

        return $"""
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Registration Approved</title>
</head>
<body style="margin: 0; padding: 0; background-color: #f1f5f9; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif; color: #1e293b; -webkit-font-smoothing: antialiased;">
    <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color: #f1f5f9; padding: 32px 12px;">
        <tr>
            <td align="center">
                <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width: 580px; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 16px rgba(0, 0, 0, 0.06); border: 1px solid #e2e8f0;">
                    <!-- HEADER -->
                    <tr>
                        <td style="background: linear-gradient(135deg, #1e40af 0%, #3b82f6 100%); padding: 36px 28px; text-align: center;">
                            <h1 style="margin: 0; color: #ffffff; font-size: 24px; font-weight: 700; letter-spacing: -0.5px;">Form Management System</h1>
                            <p style="margin: 8px 0 0 0; color: #dbeafe; font-size: 15px; font-weight: 400;">Your Account Has Been Approved!</p>
                        </td>
                    </tr>

                    <!-- CONTENT -->
                    <tr>
                        <td style="padding: 32px 28px;">
                            <p style="margin: 0 0 16px 0; font-size: 16px; color: #334155; line-height: 1.5;">
                                Hello <strong style="color: #0f172a;">{safeName}</strong>,
                            </p>
                            <p style="margin: 0 0 24px 0; font-size: 15px; color: #475569; line-height: 1.6;">
                                Your registration request has been reviewed and approved by the system administrator. Your account is now active and ready to use.
                            </p>

                            <!-- CREDENTIALS BOX -->
                            <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color: #f8fafc; border: 1px solid #e2e8f0; border-radius: 8px; margin: 0 0 24px 0;">
                                <tr>
                                    <td style="padding: 20px 24px;">
                                        <div style="font-size: 12px; font-weight: 700; text-transform: uppercase; letter-spacing: 0.05em; color: #64748b; margin-bottom: 12px;">Account Credentials</div>
                                        
                                        <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="padding: 6px 0; font-size: 14px; color: #64748b; width: 140px;">Registered Email:</td>
                                                <td style="padding: 6px 0; font-size: 14px; color: #0f172a; font-weight: 600;">{safeEmail}</td>
                                            </tr>
                                            <tr>
                                                <td style="padding: 6px 0; font-size: 14px; color: #64748b;">Assigned Role:</td>
                                                <td style="padding: 6px 0;">
                                                    <span style="display: inline-block; background-color: #eff6ff; color: #1d4ed8; border: 1px solid #bfdbfe; font-size: 12px; font-weight: 600; padding: 2px 8px; border-radius: 4px;">{safeRole}</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding: 10px 0 6px 0; font-size: 14px; color: #64748b; vertical-align: middle;">Temporary Password:</td>
                                                <td style="padding: 10px 0 6px 0; vertical-align: middle;">
                                                    <code style="background-color: #ffffff; border: 1.5px dashed #94a3b8; color: #0f172a; font-family: 'Courier New', Courier, monospace; font-size: 16px; font-weight: 700; padding: 6px 12px; border-radius: 6px; letter-spacing: 1px; display: inline-block;">{safePassword}</code>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                            <!-- SECURITY NOTICE CALLOUT -->
                            <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color: #fffbeb; border-left: 4px solid #f59e0b; border-radius: 0 6px 6px 0; margin: 0 0 28px 0;">
                                <tr>
                                    <td style="padding: 14px 18px;">
                                        <p style="margin: 0; font-size: 13px; color: #92400e; line-height: 1.5;">
                                            <strong style="color: #78350f;">Important:</strong> For security reasons, you are required to change your temporary password immediately upon your first login before you can access system features.
                                        </p>
                                    </td>
                                </tr>
                            </table>

                            <!-- CTA BUTTON -->
                            <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" style="margin: 0 0 28px 0;">
                                <tr>
                                    <td align="center">
                                        <a href="{safeLoginUrl}" target="_blank" style="background-color: #2563eb; color: #ffffff; font-size: 15px; font-weight: 600; text-decoration: none; padding: 14px 32px; border-radius: 8px; display: inline-block; box-shadow: 0 4px 6px -1px rgba(37, 99, 235, 0.2);">Login to Your Account &rarr;</a>
                                    </td>
                                </tr>
                            </table>

                            <p style="margin: 0; font-size: 13px; color: #94a3b8; line-height: 1.5; text-align: center;">
                                Or copy and paste this URL into your browser:<br>
                                <a href="{safeLoginUrl}" target="_blank" style="color: #2563eb; word-break: break-all; text-decoration: underline; font-size: 12px;">{safeLoginUrl}</a>
                            </p>
                        </td>
                    </tr>

                    <!-- FOOTER -->
                    <tr>
                        <td style="background-color: #f8fafc; border-top: 1px solid #e2e8f0; padding: 20px 28px; text-align: center;">
                            <p style="margin: 0 0 6px 0; font-size: 12px; color: #64748b;">
                                &copy; {DateTime.UtcNow.Year} Form Management System. All rights reserved.
                            </p>
                            <p style="margin: 0; font-size: 11px; color: #94a3b8;">
                                This is an automated message. Please do not reply to this email.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
""";
    }

    public static string GetPasswordResetEmail(
        string name,
        string email,
        string resetLink)
    {
        var safeName = WebUtility.HtmlEncode(name);
        var safeEmail = WebUtility.HtmlEncode(email);
        var safeResetLink = WebUtility.HtmlEncode(resetLink);

        return $"""
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Password Reset Request</title>
</head>
<body style="margin: 0; padding: 0; background-color: #f1f5f9; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif; color: #1e293b; -webkit-font-smoothing: antialiased;">
    <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color: #f1f5f9; padding: 32px 12px;">
        <tr>
            <td align="center">
                <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" style="max-width: 580px; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 16px rgba(0, 0, 0, 0.06); border: 1px solid #e2e8f0;">
                    <!-- HEADER -->
                    <tr>
                        <td style="background: linear-gradient(135deg, #0f172a 0%, #334155 100%); padding: 36px 28px; text-align: center;">
                            <h1 style="margin: 0; color: #ffffff; font-size: 24px; font-weight: 700; letter-spacing: -0.5px;">Form Management System</h1>
                            <p style="margin: 8px 0 0 0; color: #94a3b8; font-size: 15px; font-weight: 400;">Password Reset Request</p>
                        </td>
                    </tr>

                    <!-- CONTENT -->
                    <tr>
                        <td style="padding: 32px 28px;">
                            <p style="margin: 0 0 16px 0; font-size: 16px; color: #334155; line-height: 1.5;">
                                Hello <strong style="color: #0f172a;">{safeName}</strong>,
                            </p>
                            <p style="margin: 0 0 24px 0; font-size: 15px; color: #475569; line-height: 1.6;">
                                We received a request to reset the password for your Form Management System account (<strong style="color: #0f172a;">{safeEmail}</strong>).
                            </p>

                            <!-- CTA BUTTON -->
                            <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" style="margin: 0 0 24px 0;">
                                <tr>
                                    <td align="center">
                                        <a href="{safeResetLink}" target="_blank" style="background-color: #2563eb; color: #ffffff; font-size: 15px; font-weight: 600; text-decoration: none; padding: 14px 32px; border-radius: 8px; display: inline-block; box-shadow: 0 4px 6px -1px rgba(37, 99, 235, 0.2);">Reset Your Password &rarr;</a>
                                    </td>
                                </tr>
                            </table>

                            <!-- EXPIRY NOTICE -->
                            <table role="presentation" width="100%" cellpadding="0" cellspacing="0" border="0" style="background-color: #f8fafc; border: 1px solid #e2e8f0; border-radius: 8px; margin: 0 0 24px 0;">
                                <tr>
                                    <td style="padding: 14px 18px; text-align: center;">
                                        <p style="margin: 0; font-size: 13px; color: #64748b;">
                                            &#9200; This password reset link is valid for <strong style="color: #0f172a;">30 minutes</strong> only.
                                        </p>
                                    </td>
                                </tr>
                            </table>

                            <!-- SECURITY NOTICE -->
                            <p style="margin: 0 0 24px 0; font-size: 14px; color: #64748b; line-height: 1.5;">
                                If you did not request a password reset, please disregard this email. Your password will remain unchanged and your account remains safe.
                            </p>

                            <p style="margin: 0; font-size: 13px; color: #94a3b8; line-height: 1.5; text-align: center;">
                                Or copy and paste this URL into your browser:<br>
                                <a href="{safeResetLink}" target="_blank" style="color: #2563eb; word-break: break-all; text-decoration: underline; font-size: 12px;">{safeResetLink}</a>
                            </p>
                        </td>
                    </tr>

                    <!-- FOOTER -->
                    <tr>
                        <td style="background-color: #f8fafc; border-top: 1px solid #e2e8f0; padding: 20px 28px; text-align: center;">
                            <p style="margin: 0 0 6px 0; font-size: 12px; color: #64748b;">
                                &copy; {DateTime.UtcNow.Year} Form Management System. All rights reserved.
                            </p>
                            <p style="margin: 0; font-size: 11px; color: #94a3b8;">
                                This is an automated message. Please do not reply to this email.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
""";
    }
}
