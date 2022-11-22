// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Baseline.Services.Identity.MainModule.Consent;

namespace Baseline.Services.Identity.MainModule.Device
{
    public class DeviceAuthorizationViewModel : ConsentViewModel
    {
        public string UserCode { get; set; }
        public bool ConfirmUserCode { get; set; }
    }
}