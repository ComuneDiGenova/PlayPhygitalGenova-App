using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAppInfoTab : MonoBehaviour
{
    [SerializeField] ExternalSitePage externalSitePage;

    [SerializeField] GameObject appInfoProPanel;
    [SerializeField] GameObject appInfoProButton;
    [SerializeField] GameObject appInfoProCloseButton;

    [SerializeField] GameObject appPrivacyProPanel;
    [SerializeField] GameObject appPrivacyProButton;
    [SerializeField] GameObject appPrivacyProCloseButton;

    [SerializeField] GameObject appTransactionProPanel;
    [SerializeField] GameObject appTransactionProButton;
    [SerializeField] GameObject appTransactionCloseProButton;

    [SerializeField] GameObject appInfoPanel;
    [SerializeField] GameObject appInfoButton;
    [SerializeField] GameObject appInfoCloseButton;

    [SerializeField] GameObject appPrivacyPanel;
    [SerializeField] GameObject appPrivacyButton;
    [SerializeField] GameObject appPrivacyCloseButton;

    [SerializeField] GameObject appTransactionPanel;
    [SerializeField] GameObject appTransactionButton;
    [SerializeField] GameObject appTransactionCloseButton;

    public void ActiveAppProInfo()
    {
        appInfoProPanel.SetActive(true);
        appInfoProCloseButton.SetActive(true);
    }

    public void CloseAppProInfo()
    {
        if (appInfoProPanel.activeSelf)
        {
            appInfoProPanel.SetActive(false);
            appInfoProCloseButton.SetActive(false);
        }
    }

    public void ActivePrivacyProInfo()
    {
        appPrivacyProPanel.SetActive(true);
        appPrivacyProCloseButton.SetActive(true);
    }

    public void ClosePrivacyProInfo()
    {
        if (appPrivacyProPanel.activeSelf)
        {
            appPrivacyProPanel.SetActive(false);
            appPrivacyProCloseButton.SetActive(false);     
        }
    }

    public void AciveTransactionProInfo()
    {
        appTransactionProPanel.SetActive(true);
        appTransactionCloseProButton.SetActive(true);
    }

    public void CloseTransactionProInfo()
    {
        if (appTransactionProPanel.activeSelf)
        {
            appTransactionProPanel.SetActive(false);
            appTransactionCloseProButton.SetActive(true);
        }
    }

    public void ActiveAppInfo()
    {
        appInfoPanel.SetActive(true);
        appInfoCloseButton.SetActive(true);
    }

    public void CloseAppInfo()
    {
        if (appInfoPanel.activeSelf)
        {
            appInfoPanel.SetActive(false);
            appInfoCloseButton.SetActive(false);
        }
    }

    public void ActivePrivacyInfo()
    {
        ExternalSitePage.OpenPrivacy();
        /*
        appPrivacyPanel.SetActive(true);
        appPrivacyCloseButton.SetActive(true);
        */
    }

    public void ClosePrivacyInfo()
    {
        if (appPrivacyPanel.activeSelf)
        {
            appPrivacyPanel.SetActive(false);
            appPrivacyCloseButton.SetActive(false);
        }
    }

    public void AciveTransactionInfo()
    {
        //ExternalSitePage.OpenTransactions();
        
        appTransactionPanel.SetActive(true);
        appTransactionCloseButton.SetActive(true);
        
    }

    public void CloseTransactionInfo()
    {
        if (appTransactionPanel.activeSelf)
        {
            appTransactionPanel.SetActive(false);
            appTransactionCloseButton.SetActive(true);
        }
    }
}
