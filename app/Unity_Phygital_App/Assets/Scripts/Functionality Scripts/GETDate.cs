using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Transactions;

public class GETDate : MonoBehaviour
{
    [SerializeField] TMP_InputField dateIF;

    [SerializeField] TMP_Dropdown dayDropDown;
    [SerializeField] TMP_Dropdown monthDropDown;
    [SerializeField] TMP_Dropdown yearDropDown;

    [SerializeField] TMP_Text dayDropDownLabel;
    [SerializeField] TMP_Text monthDropDownLabel;
    [SerializeField] TMP_Text yearDropDownLabel;

    [SerializeField] GameObject dateSelector;

    [SerializeField] GameObject dateErrorTab;

    private List<string> dayList = new List<string>();
    private List<string> monthList = new List<string>();
    private List<string> yearList = new List<string>();
    private List<string> ventinoveDay = new List<string>();
    private List<string> trentaDay = new List<string>();
    private List<string> trentunoDay = new List<string>();


    string selectedYear;
    string selectedMonth;
    string selectedDay;

    string completeDate;

    void Start()
    {
        System.DateTime currentDate = System.DateTime.Now;

        dateIF.text = currentDate.ToString("dd-MM-yyyy");

        CalculateDropDown();
    }

    public void ActiveDateSelector()
    {
        dateSelector.SetActive(true);
    }

    public void ChangeDate()
    {
        System.DateTime currentDate = System.DateTime.Now;

        GetDropDownValue();

        if (int.Parse(selectedMonth) > currentDate.Month || int.Parse(selectedMonth) == currentDate.Month && int.Parse(selectedDay) > currentDate.Day) 
        {
            dateErrorTab.SetActive(true);
            return;
        }

        SetDate(selectedYear, selectedMonth, selectedDay, out completeDate);
        dateIF.text = completeDate;

        dateSelector.SetActive(false);
    }

    void CalculateDropDown()
    {
        /*
        for (int i = 20; i <= 30; i++)
            yearList.Add(i.ToString());
        */

        yearList.Add(23.ToString());

        for (int i = 1; i <= 12; i++)
            monthList.Add(i.ToString());
    
        for (int i = 1; i <= 31; i++)
            dayList.Add(i.ToString());

        ventinoveDay.Add(29.ToString());

        trentaDay.Add(29.ToString());
        trentaDay.Add(30.ToString());

        trentunoDay.Add(29.ToString());
        trentunoDay.Add(30.ToString());
        trentunoDay.Add(31.ToString());

        PopulateDropDown();

        System.DateTime currentDate = System.DateTime.Now;

        // Convert each part to an integer
        dayDropDown.value = currentDate.Day - 1;
        monthDropDown.value = currentDate.Month - 1;
    }

    void PopulateDropDown()
    {
        yearDropDown.AddOptions(yearList);
        monthDropDown.AddOptions(monthList);
        dayDropDown.AddOptions(dayList);
    }

    void PopulateDay()
    {
        dayDropDown.AddOptions(dayList);
    }

    void GetDropDownValue()
    {     
        selectedYear = yearDropDownLabel.text;
        selectedMonth = monthDropDownLabel.text;
        selectedDay = dayDropDownLabel.text;

        if (selectedMonth.Length <= 1) 
            selectedMonth = "0" + selectedMonth;

        if (selectedDay.Length <= 1)
            selectedDay = "0" + selectedDay;
    }

    void SetDate(string year, string month, string day, out string completeDate)
    {
        completeDate = day + "-" + month + "-" + year;
    }

    public void CheckDayInMonth()
    {
        PopulateDay();

        if (monthDropDownLabel.text == "1" || monthDropDownLabel.text == "3" || monthDropDownLabel.text == "5" || monthDropDownLabel.text == "7" || monthDropDownLabel.text == "8" || monthDropDownLabel.text == "10" || monthDropDownLabel.text == "12")
        {
            if (dayDropDown.options.Count >= 30)
            {
                for (int i = dayDropDown.options.Count; i >= 29; i--)
                {
                    dayDropDown.options.RemoveAt(i - 1);
                }
                dayDropDown.AddOptions(trentunoDay);
            }
        }

        if (monthDropDownLabel.text == "4" || monthDropDownLabel.text == "6" || monthDropDownLabel.text == "9" || monthDropDownLabel.text == "11")
        {
            if (dayDropDown.options.Count >= 30)
            {
                for (int i = dayDropDown.options.Count; i >= 29; i--)
                {
                    dayDropDown.options.RemoveAt(i - 1);
                }
                dayDropDown.AddOptions(trentaDay);
            }

            if (dayDropDown.value == 30)
                dayDropDown.value = 29;
        }

        if (monthDropDownLabel.text == "2")         
        {
            if (yearDropDownLabel.text == "20" || yearDropDownLabel.text == "24" || yearDropDownLabel.text == "28")
            {
                if (dayDropDown.options.Count >= 30)
                {
                    for (int i = dayDropDown.options.Count - 1; i >= 28; i--)
                    {
                        dayDropDown.options.RemoveAt(i);
                    }
                    dayDropDown.AddOptions(ventinoveDay);
                }

                if (dayDropDown.value >= 29)
                    dayDropDown.value = 29;
            }
            else
            {
                Debug.Log(yearDropDownLabel.text);
                if (dayDropDown.options.Count >= 30)
                {
                    for (int i = dayDropDown.options.Count; i >= 29; i--)
                    {
                        dayDropDown.options.RemoveAt(i - 1);
                    }
                }

                if (dayDropDown.value >= 27)
                    dayDropDown.value = 27;
            }
        }
    }

    public void CloseErrorTab()
    {
        dateErrorTab.SetActive(false);
    }

}

