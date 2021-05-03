using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

#if RECEIPT_VALIDATION
using UnityEngine.Purchasing.Security;
#endif

public class GoogleIAPManager : MonoBehaviour, IStoreListener
{
	private static GoogleIAPManager m_pInstance = null;
	public static GoogleIAPManager Instance { get { return m_pInstance; } }

	private IStoreController m_StoreController = null;
	private IExtensionProvider m_StoreExtensionProvider = null;

	public bool m_IsInitialize = false;

	private void Awake()
	{
		m_pInstance = this;
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	public void OnInitialize()
	{
#if UNITY_ANDROID
		Init();
#endif
	}

	private void Init()
	{
		ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		int nNumData = ExcelDataManager.Instance.m_pExcelData_Google_IAPData.GetNumData();

		for (int i = 0; i < nNumData; ++i)
		{
			ExcelData_Google_IAPDataInfo pIAPDataInfo = ExcelDataManager.Instance.m_pExcelData_Google_IAPData.GetIAPDataInfo_byIndex(i);

			if (pIAPDataInfo.m_eIAPType != eIAPType.Single_Free && pIAPDataInfo.m_eIAPType != eIAPType.Single_Ads)
			{
				builder.AddProduct(pIAPDataInfo.m_strProduct_ID, pIAPDataInfo.m_ProductType, new IDs() { { pIAPDataInfo.m_strProduct_Name, GooglePlay.Name }, });

				OutputLog.Log("GoogleIAPManager : Init, " + pIAPDataInfo.m_ProductType + " , " + pIAPDataInfo.m_ProductType.ToString() + " , " + pIAPDataInfo.m_strProduct_Name);
			}
		}

		OutputLog.Log("GoogleIAPManager : Init");
		UnityPurchasing.Initialize(this, builder);
	}

	// UnityPurchasing.Initialize 에 대한 콜백
	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		OutputLog.Log("GoogleIAPManager : OnInitialized 01");

		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;

		int nNumData = ExcelDataManager.Instance.m_pExcelData_Google_IAPData.GetNumData();

		for (int i = 0; i < nNumData; ++i)
		{
			ExcelData_Google_IAPDataInfo pIAPDataInfo = ExcelDataManager.Instance.m_pExcelData_Google_IAPData.GetIAPDataInfo_byIndex(i);

			if (pIAPDataInfo.m_eIAPType != eIAPType.Single_Free && pIAPDataInfo.m_eIAPType != eIAPType.Single_Ads)
			{
				Product pProduct = m_StoreController.products.WithID(pIAPDataInfo.m_strProduct_ID);

				if (pProduct != null)
				{
					pIAPDataInfo.m_dePrice = pProduct.metadata.localizedPrice;
					pIAPDataInfo.m_strCountryCode = pProduct.metadata.isoCurrencyCode;

					OutputLog.Log("GoogleIAPManager : OnInitialized 02, " + pIAPDataInfo.m_dePrice.ToString() + " , " + pIAPDataInfo.m_strCountryCode);
				}
			}
		}

		m_IsInitialize = true;

		OutputLog.Log("GoogleIAPManager : OnInitialized 03");
	}

	// UnityPurchasing.Initialize 에 대한 콜백
	public void OnInitializeFailed(InitializationFailureReason error)
	{
		OutputLog.Log("GoogleIAPManager : OnInitializeFailed : " + error.ToString());
	}

	public void Buy(ExcelData_Google_IAPDataInfo pIAPDataInfo)
	{
#if UNITY_EDITOR
		AppInstance.Instance.m_pEventDelegateManager.OnGoogleProcessPurchaseDone(pIAPDataInfo);
		return;
#elif UNITY_ANDROID
		try
		{
			if (m_StoreController != null && m_StoreExtensionProvider != null)
			{
				Product product = m_StoreController.products.WithID(pIAPDataInfo.m_strProduct_ID);

				if (product != null && product.availableToPurchase)
				{
					m_StoreController.InitiatePurchase(product);
				}
				else
				{
					OutputLog.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
					AppInstance.Instance.m_pEventDelegateManager.OnGoogleProcessPurchaseFailed(pIAPDataInfo);
				}
			}
			else
			{
				OutputLog.Log("BuyProductID FAIL. Not initialized.");
				AppInstance.Instance.m_pEventDelegateManager.OnGoogleProcessPurchaseFailed(pIAPDataInfo);
			}
		}
		catch (Exception e)
		{
			OutputLog.Log("BuyProductID: FAIL. Exception during purchase. " + e);
			AppInstance.Instance.m_pEventDelegateManager.OnGoogleProcessPurchaseFailed(pIAPDataInfo);
		}
#endif
	}

	// BuyProductID 에 대한 콜백
	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		ExcelData_Google_IAPDataInfo pIAPDataInfo = ExcelDataManager.Instance.m_pExcelData_Google_IAPData.GetIAPDataInfo_byProductID(args.purchasedProduct.definition.id);

		if (pIAPDataInfo != null)
		{
			AppInstance.Instance.m_pEventDelegateManager.OnGoogleProcessPurchaseDone(pIAPDataInfo);
		}
		else
		{
			AppInstance.Instance.m_pEventDelegateManager.OnGoogleProcessPurchaseFailed(pIAPDataInfo);

			return PurchaseProcessingResult.Pending;
		}

		return PurchaseProcessingResult.Complete;
	}

	// BuyProductID 에 대한 콜백
	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		ExcelData_Google_IAPDataInfo pIAPDataInfo = ExcelDataManager.Instance.m_pExcelData_Google_IAPData.GetIAPDataInfo_byProductID(product.definition.id);

		AppInstance.Instance.m_pEventDelegateManager.OnGoogleProcessPurchaseFailed(pIAPDataInfo);
	}
}