<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Petanque.Azure" generation="1" functional="0" release="0" Id="3f51f308-f2da-422d-9f7c-f560ad94e162" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="Petanque.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="Petanque.Web:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Petanque.Azure/Petanque.AzureGroup/LB:Petanque.Web:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/Petanque.Azure/Petanque.AzureGroup/LB:Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/Petanque.Azure/Petanque.AzureGroup/MapCertificate|Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="Petanque.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Petanque.Azure/Petanque.AzureGroup/MapPetanque.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/Petanque.Azure/Petanque.AzureGroup/MapPetanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/Petanque.Azure/Petanque.AzureGroup/MapPetanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/Petanque.Azure/Petanque.AzureGroup/MapPetanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/Petanque.Azure/Petanque.AzureGroup/MapPetanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/Petanque.Azure/Petanque.AzureGroup/MapPetanque.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="Petanque.WebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Petanque.Azure/Petanque.AzureGroup/MapPetanque.WebInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:Petanque.Web:Endpoint1">
          <toPorts>
            <inPortMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.Web/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.Web/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapPetanque.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.Web/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapPetanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapPetanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapPetanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapPetanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapPetanque.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.Web/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapPetanque.WebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.WebInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Petanque.Web" generation="1" functional="0" release="0" software="C:\Users\Etienne\Documents\GitHub\PetanqueMVC\Petanque.Azure\csx\Release\roles\Petanque.Web" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/Petanque.Azure/Petanque.AzureGroup/SW:Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Petanque.Web&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Petanque.Web&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.WebInstances" />
            <sCSPolicyFaultDomainMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.WebFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="Petanque.WebFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="Petanque.WebInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="a99a1b2e-0a4d-4218-b885-deab16b056ac" ref="Microsoft.RedDog.Contract\ServiceContract\Petanque.AzureContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="8bc42096-c3a7-4d1b-8ed7-ea299d60b5f3" ref="Microsoft.RedDog.Contract\Interface\Petanque.Web:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.Web:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="47e3b89d-26d3-4cd8-8d10-5f3f04275e65" ref="Microsoft.RedDog.Contract\Interface\Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/Petanque.Azure/Petanque.AzureGroup/Petanque.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>