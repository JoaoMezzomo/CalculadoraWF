using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculadorWF
{
    public partial class Form1 : Form
    {
        string ValoresDigitados;

        public Form1()
        {
            InitializeComponent();
        }

        private bool ValidarEntradaDeSinalNoVisor()
        {
            if (!string.IsNullOrEmpty(txtVisor.Text) && txtVisor.Text[txtVisor.Text.Length - 1] != ',' && !VerificarOperacaoValida(txtVisor.Text[txtVisor.Text.Length - 1].ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidarEntradaDeNumeroNoVisor() 
        {
            if (string.IsNullOrEmpty(txtVisor.Text) || txtVisor.Text[txtVisor.Text.Length - 1] != '%')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ColocarPorcentagem() 
        {
            string[] valores = ValoresDigitados.Split('_');

            if (valores.Length >= 1)
            {
                if (double.TryParse(valores[valores.Length - 1], out double saida))
                {
                    txtVisor.Text += "%";
                    ValoresDigitados += "_%"; 
                }
            }
        }

        private void InverterSinal() 
        {
            string[] valores = ValoresDigitados.Split('_');

            if (valores.Length >= 1)
            {
                if (double.TryParse(valores[valores.Length - 1], out double saida))
                {
                    string novoValor = valores[valores.Length - 1];

                    if (novoValor.Contains("-"))
                    {
                        novoValor = novoValor.Replace("-", "");
                    }
                    else
                    {
                        novoValor = "-" + novoValor;
                    }

                    valores[valores.Length - 1] = novoValor;

                    ValoresDigitados = "";
                    txtVisor.Clear();

                    foreach (string valor in valores)
                    {
                        if (VerificarOperacaoValida(valor))
                        {
                            ValoresDigitados +=  "_" + valor + "_";
                        }
                        else
                        {
                            ValoresDigitados += valor;
                        }

                        if (valor.Contains("-") && valor.Length > 1)
                        {
                            txtVisor.Text += "(" + valor + ")";
                        }
                        else
                        {
                            txtVisor.Text += valor;
                        }
                    }
                }
            }
        }

        private void CalcularResultado() 
        {
            string[] valores = ValoresDigitados.Split('_');
            double valor1 = 0;
            string valor1Texto = "";
            double valor2 = 0;
            string valor2Texto = "";
            char operacao = ' ';
            bool porcentagem = false;

            foreach (string valor in valores)
            {
                if (!string.IsNullOrEmpty(valor))
                {
                    if (valor == "%")
                    {
                        porcentagem = true;
                    }
                    else
                    {
                        porcentagem = false;
                    }

                    if (!string.IsNullOrEmpty(valor1Texto) && !string.IsNullOrEmpty(valor2Texto))
                    {
                        valor1 = Convert.ToDouble(valor1Texto);
                        valor2 = Convert.ToDouble(valor2Texto);
                        valor1 = Calcular(valor1, valor2, operacao, porcentagem);
                        valor1Texto = valor1.ToString();
                        valor2 = 0;
                        valor2Texto = "";
                        operacao = ' ';
                        porcentagem = false;
                    }

                    if (double.TryParse(valor, out double teste))
                    {
                        if (valor1 == 0)
                        {
                            valor1Texto += valor;
                        }
                        else
                        {
                            valor2Texto += valor;
                        }
                    }
                    else
                    {
                        if (VerificarOperacaoValida(valor))
                        {
                            operacao = Convert.ToChar(valor);

                            if (valor1 == 0)
                            {
                                valor1 = Convert.ToDouble(valor1Texto);
                            }
                        }
                    } 
                }
            }

            if (!string.IsNullOrEmpty(valor1Texto) && !string.IsNullOrEmpty(valor2Texto))
            {
                valor1 = Convert.ToDouble(valor1Texto);
                valor2 = Convert.ToDouble(valor2Texto);
                valor1 = Calcular(valor1, valor2, operacao, porcentagem);
                valor1Texto = valor1.ToString();
                valor2 = 0;
                valor2Texto = "";
                operacao = ' ';
                porcentagem = false;
            }

            Reiniciar();
            MontarValor(valor1.ToString());
        }

        private bool VerificarOperacaoValida(string valor) 
        {
            if (valor == "+" || valor == "-" || valor == "*" || valor == "/")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private double Calcular(double valor1, double valor2, char operacao, bool porcentagem) 
        {
            double resultado = 0;

            if (porcentagem)
            {
                valor2 = valor2 * valor1;
                valor2 = valor2 / 100;
            }

            switch (operacao)
            {
                case '+':
                    resultado = valor1 + valor2;
                    break;
                case '-':
                    resultado = valor1 - valor2;
                    break;
                case '*':
                    resultado = valor1 * valor2;
                    break;
                case '/':
                    resultado = valor1 / valor2;
                    break;
            }

            return resultado;
        }

        private void Apagar() 
        {
            if (txtVisor.Text.Length >= 1 && ValoresDigitados.Length >= 1)
            {
                txtVisor.Text = txtVisor.Text.Remove(txtVisor.Text.Length - 1);

                if (ValoresDigitados[ValoresDigitados.Length - 1] == '_')
                {
                    ValoresDigitados = ValoresDigitados.Remove(ValoresDigitados.Length - 3);
                }
                else if (ValoresDigitados[ValoresDigitados.Length - 1] == '%')
                {
                    ValoresDigitados = ValoresDigitados.Remove(ValoresDigitados.Length - 2);
                }
                else
                {
                    ValoresDigitados = ValoresDigitados.Remove(ValoresDigitados.Length - 1);
                }
            }
        }

        private void Reiniciar() 
        {
            ValoresDigitados = "";
            txtVisor.Clear();
        }

        private void MostrarNaTela(string valorDigitado) 
        {
            txtVisor.Text += valorDigitado;
        }

        private void MontarValor(string valorDigitado) 
        {
            if (VerificarOperacaoValida(valorDigitado))
            {
                ValoresDigitados += "_" + valorDigitado + "_";
            }
            else
            {
                ValoresDigitados += valorDigitado;
            }
            MostrarNaTela(valorDigitado);
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeNumeroNoVisor())
            {
                return;
            }

            MontarValor("0");
        }

        private void btnUm_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeNumeroNoVisor())
            {
                return;
            }

            MontarValor("1");
        }

        private void btnDois_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeNumeroNoVisor())
            {
                return;
            }

            MontarValor("2");
        }

        private void btnTres_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeNumeroNoVisor())
            {
                return;
            }

            MontarValor("3");
        }

        private void btnQuatro_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeNumeroNoVisor())
            {
                return;
            }

            MontarValor("4");
        }

        private void btnCinco_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeNumeroNoVisor())
            {
                return;
            }

            MontarValor("5");
        }

        private void btnSeis_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeNumeroNoVisor())
            {
                return;
            }

            MontarValor("6");
        }

        private void btnSete_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeNumeroNoVisor())
            {
                return;
            }

            MontarValor("7");
        }

        private void btnOito_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeNumeroNoVisor())
            {
                return;
            }

            MontarValor("8");
        }

        private void btnNove_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeNumeroNoVisor())
            {
                return;
            }

            MontarValor("9");
        }

        private void btnPonto_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeSinalNoVisor())
            {
                return;
            }

            MontarValor(",");
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            Reiniciar();
        }

        private void btnApaga_Click(object sender, EventArgs e)
        {
            Apagar();
        }

        private void btnAdicao_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeSinalNoVisor())
            {
                return;
            }

            MontarValor("+");
        }

        private void btnSubtracao_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeSinalNoVisor())
            {
                return;
            }

            MontarValor("-");
        }

        private void btnMultiplicacao_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeSinalNoVisor())
            {
                return;
            }

            MontarValor("*");
        }

        private void btnDivisao_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeSinalNoVisor())
            {
                return;
            }

            MontarValor("/");
        }

        private void btnIgual_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ValoresDigitados))
                {
                    CalcularResultado();
                }
            }
            catch (Exception)
            {
                Reiniciar();
            }
        }

        private void btnSinal_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeSinalNoVisor())
            {
                return;
            }

            InverterSinal();
        }

        private void btnPorcentagem_Click(object sender, EventArgs e)
        {
            if (!ValidarEntradaDeSinalNoVisor())
            {
                return;
            }

            ColocarPorcentagem();
        }
    }
}
