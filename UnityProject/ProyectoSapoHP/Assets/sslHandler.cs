using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class SSLHandler : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}