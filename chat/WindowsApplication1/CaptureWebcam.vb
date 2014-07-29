Option Explicit On

Public Class CaptureWebcam
    Const ws_visible = &H10000000
    Const ws_child = &H40000000
    Const WM_USER = 1024
    Const WM_CAP_EDIT_COPY = WM_USER + 30
    Const wm_cap_driver_connect = WM_USER + 10
    Const wm_cap_set_preview = WM_USER + 50
    Const wm_cap_set_overlay = WM_USER + 51
    Const WM_CAP_SET_PREVIEWRATE = WM_USER + 52
    Const WM_CAP_SEQUENCE = WM_USER + 62
    Const WM_CAP_SINGLE_FRAME_OPEN = WM_USER + 70
    Const WM_CAP_SINGLE_FRAME_CLOSE = WM_USER + 71
    Const WM_CAP_SINGLE_FRAME = WM_USER + 72
    Const DRV_USER = &H4000
    Const DVM_DIALOG = DRV_USER + 100
    Const PREVIEWRATE = 30

    Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hWnd As Long, ByVal wMsg As Long, ByVal wParam As Long, ByVal lParam As Long) As Long
    Private Declare Function capCreateCaptureWindow Lib "avicap32.dll" Alias "capCreateCaptureWindowA" (ByVal a As String, ByVal b As Long, ByVal c As Integer, ByVal d As Integer, ByVal e As Integer, ByVal f As Integer, ByVal g As Long, ByVal h As Integer) As Long

    Dim hwndc As Long
    Dim saveflag As Integer
    Dim pictureindex As Integer

    Dim temp As String

    Dim picture As System.Drawing.Image

    Private Sub Form_Load()
        On Error GoTo handler
        hwndc = capCreateCaptureWindow("CaptureWindow", ws_child Or ws_visible, 0, 0, picture.Width, picture.Height, picture.VerticalResolution, 0)
        If (hwndc <> 0) Then
            temp = SendMessage(hwndc, wm_cap_driver_connect, 0, 0)
            temp = SendMessage(hwndc, wm_cap_set_preview, 1, 0)
            temp = SendMessage(hwndc, WM_CAP_SET_PREVIEWRATE, PREVIEWRATE, 0)

            temp = SendMessage(Me.hwndc, WM_CAP_EDIT_COPY,  1, 0)
            picture = Clipboard.GetData(temp)
        Else
            MsgBox("Unable to capture video.", vbCritical)
        End If
handler:
    End Sub
End Class
