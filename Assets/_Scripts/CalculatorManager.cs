using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalculatorManager : MonoBehaviour
{
    public AudioClip sfx_Click;
    public AudioClip sfx_Enter;
    public enum Operator
    {
        Add, Subtract, Multiply, Divide
    }
    private Operator _operator;
    private Operator _prevOperator;

    private double leftOperand = 0;
    private double rightOperand = 0;
    private double result;

    public TextMeshProUGUI TopDisplayField;
    public TextMeshProUGUI OperatorField;
    public TextMeshProUGUI BottomDisplayField;

    public GameObject noticePanel;
    public TextMeshProUGUI noticeInfo;

    private bool _nextField = false;

    private void Start()
    {
        TopDisplayField.text = "";
        OperatorField.text = "";
        BottomDisplayField.text = "";
    }
    public void GetInput(int operand)
    {
        GetComponent<AudioSource>().PlayOneShot(sfx_Click);
        if (_nextField)
        {
            BottomDisplayField.text += operand.ToString();
        }
        else
            TopDisplayField.text += operand.ToString();
    }

    public void GetOperator(string _op)
    {
        if (_op != "=" && OperatorField.text != string.Empty)
        {
            _operator = _prevOperator;
            Equate();
        }
        _nextField = true;
        if (_op == "+")
            _operator = Operator.Add;
        else if (_op == "-")
            _operator = Operator.Subtract;
        else if (_op == "x")
            _operator = Operator.Multiply;
        else if (_op == "/")
            _operator = Operator.Divide;
        else
        {
            Equate();
            return;
        }
        _prevOperator = _operator;
        OperatorField.SetText(_op);
    }
    public double Add() => leftOperand + rightOperand;
    public double Subtract() => leftOperand - rightOperand;
    public double Multiply() => leftOperand * rightOperand;
    public double Divide()
    {
        // more accurately done with try and catch blocks. 
        if (rightOperand == 0)
        {
            ClearScr();
            // Pop up error. 
            noticeInfo.SetText("CAN'T DIVIDE BY 0");
            noticePanel.SetActive(true);
            return 0;
        }
        return leftOperand / rightOperand;
    }
    public void Negate()
    {
        GetComponent<AudioSource>().PlayOneShot(sfx_Enter);
        if (_nextField)
            BottomDisplayField.text = (double.Parse(BottomDisplayField.text) * -1).ToString();
        else
            TopDisplayField.text = (double.Parse(TopDisplayField.text) * -1).ToString(); 
    }
    public void Equate()
    {
        leftOperand = double.Parse(TopDisplayField.text);
        rightOperand = double.Parse(BottomDisplayField.text);

        switch (_operator)
        {
            case Operator.Add:
                result = Add();
                break;
            case Operator.Subtract:
                result = Subtract();
                break;
            case Operator.Multiply:
                result = Multiply();
                break;
            case Operator.Divide:
                result = Divide();
                break;
            default:
                break;
        }

        //print("Result can follow from subsequent operators not only equal"); 
        GetComponent<AudioSource>().PlayOneShot(sfx_Enter);
        TopDisplayField.text = result.ToString();
        BottomDisplayField.text = string.Empty;
        OperatorField.text = string.Empty;
    }
    public void ClearScr()
    {
        GetComponent<AudioSource>().PlayOneShot(sfx_Enter);
        BottomDisplayField.SetText(string.Empty);
        TopDisplayField.SetText(string.Empty);
        _nextField = false;
        leftOperand = 0;
        rightOperand = 0;
    }
}
