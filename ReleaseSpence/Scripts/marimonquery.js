//validar_rut: valida rut chileno en fonrmato 12345678-k
$.fn.RutChileno=function(rut, error)
{
	//empieza con 1-9, siguen 6 o 7 digitos 0-9, un guion, termina con numero 0-9 o k
	if(!rut.val().match(/^[1-9]{1}[0-9]{6,7}[-]{1}[0-9k]{1}$/))
	{
		error.html("<font color='#d59392'>formato incorrecto</font>");
		rut.css("background-color", '#d59392');
		return false;
	}
	else
	{
		var multiplicador = 2;
		var suma = 0;
		for (i = rut.val().length - 3; i >= 0; i--)
		{
			suma += rut.val().charAt(i) * multiplicador;
			multiplicador++;
			if (multiplicador > 7)
				multiplicador = 2;
		}
		var verif = 11 - (suma % 11);
		if (verif == "10")
		{
			verif = "k";
		}
		else if (verif == 11)
		{
			verif = 0;
		}
		if (verif == rut.val().charAt(rut.val().length - 1))
		{
			rut.css("background-color", "#ccffcc");
			error.html("");
			return true;
		}
		else
		{
			error.html("<font color='#d59392'>RUT incorrecto</font>");
			rut.css("background-color", '#d59392');
			return false;
		}
	}
}
//SoloLetras: verifica que solo se ingresen letras y espacios
$.fn.SoloLetras=function(objeto, DivError)
{
	//empieza con letras a-z A-Z, siguen letras a-z A-Z y espacios, termina con letras a-z A-Z
	if(!objeto.val().match(/^[a-zA-Z]+[a-zA-Z\s]+[a-zA-Z]+$/))
	{
		DivError.html("<font color='#d59392'>Solo se permite letras(sin acentos) y espacios<br>No se permiten espacios al inicio ni al final</font>");
		objeto.css("background-color", '#d59392');
		return false;
	}
	//busca espacios dobles
	else if(objeto.val().match(/[\s]{2}/))
	{
		DivError.html("<font color='#d59392'>No se permiten espacios dobles</font>");
		objeto.css("background-color", '#d59392');
		return false;
	}
	else
	{
		DivError.html("");
		objeto.css("background-color", "#ccffcc");
		return true;
	}
}
//Usernames: verifica que solo se ingresen letras y numeros
$.fn.Usernames=function(objeto, DivError)
{
	if(!objeto.val().match(/^[\w]+$/))
	{
		DivError.html("<font color='#d59392'>Solo se permite letras y numeros</font>");
		objeto.css("background-color", '#d59392');
		return false;
	}
	else
	{
		DivError.html("");
		objeto.css("background-color", "#ccffcc");
		return true;
	}
}
//Alfanumerico: solo letras y numeros con espacios, ._-
$.fn.Alfanumerico=function(objeto, DivError)
{
	if(!objeto.val().match(/^[\w\s\.\-_]+$/))
	{
		DivError.html("<font color='#d59392'>Solo se permite letras, numeros y puntuaciones(- _ .)</font>");
		objeto.css("background-color", '#d59392');
		return false;
	}
	else
	{
		DivError.html("");
		objeto.css("background-color", "#ccffcc");
		return true;
	}
}
//Passwords: verifica que las contraseñas tenga un largo minimo y maximo ademas de coincidir y contener caracteres alfanumericos o _.-
$.fn.Passwords=function(pass1, pass2, largoMinimo, largoMaximo, DivError)
{
	if(pass1.val().length<largoMinimo || pass1.val().length>largoMaximo)
	{
		pass1.css("background-color", '#d59392');
		DivError.html("<font color='#d59392'>Contrase&ntilde;a debe tener entre<br>" + largoMinimo + " y " + largoMaximo + " caracteres</font>");
		return false;
	}
	//verifica que solo contiene caracteres alfanumericos, guiones, guion bajo y puntos(-_.)
	// \w: alfanumericos y guion bajo
	else if(!pass1.val().match(/^[\w.-]+$/))
	{
		pass1.css("background-color", '#d59392');
		DivError.html("<font color='#d59392'>Solo se permiten caracteres alfanumericos, puntos y guiones</font>");
		return false;
	}
	else if(pass1.val() != pass2.val())
	{
		pass2.css("background-color", '#d59392');
		DivError.html("<font color='#d59392'>Las Contrase&ntildes no coinciden</font>");
		return false;
	}
	else
	{
		pass1.css("background-color", "#ccffcc");
		pass2.css("background-color", "#ccffcc");
		DivError.html("");
		return true;
	}
}
//Emails: verifica que sea una direccion de correo electronico
$.fn.Emails=function(objeto, DivError)
{
	//verifica que empize con alfanumerico, sigue con alfanumerico puntos y guiones, un @, sigue con letras a-z A-Z y puntos, termina con a-z
	if(objeto.val().match(/^[a-zA-Z0-9]{1}[\w-.]+[@]{1}[a-zA-Z.]+[a-z]+$/))
	{
		DivError.html("");
		objeto.css("background-color", "#ccffcc");
		return true;
	}
	else
	{
		DivError.html("<font color='#d59392'>email invalido</font>");
		objeto.css("background-color", '#d59392');
		return false;
	}
}
//Telefonos: verifica que solo se ingresen numeros y guiones
$.fn.Telefonos=function(objeto, DivError)
{
	//empieza con numeros, si se ingresan guines deben estar separados por numeros, maximo 2 guines, termina con numeros
	if(objeto.val().match(/^[0-9]+([-]?[0-9]+[-]?)?[0-9]+$/))
	{
		DivError.html("");
		objeto.css("background-color", "#ccffcc");
		return true;
	}
	else
	{
		DivError.html("<font color='#d59392'>Telefono invalido</font>");
		objeto.css("background-color", '#d59392');
		return false;
	}
}
//Obligatorio: verifica que el campo no este vacio o relleno con espacios en blanco
$.fn.Obligatorio=function(objeto, DivError, nombre)
{
	if(objeto.val().trim() != "")
	{
		DivError.html("");
		objeto.css("background-color", "#ccffcc");
		return true;
	}
	else
	{
		DivError.html("<font color='#d59392'>"+nombre+" es obligatorio</font>");
		objeto.css("background-color", '#d59392');
		return false;
	}
}
//SelectOb: verifica que el value seleccionado no sea -1
$.fn.SelectOb=function(objeto, DivError, nombre)
{
	if(objeto.val() != -1)
	{
		DivError.html("");
		objeto.css("background-color", "#ccffcc");
		return true;
	}
	else
	{
		DivError.html("<font color='#d59392'>"+nombre+" es obligatorio</font>");
		objeto.css("background-color", '#d59392');
		return false;
	}
}
//NumeroNatural: verifica que el campo contenga un numero entero positivo
$.fn.NumeroNatural=function(objeto, DivError)
{
	//empieza y termina con numeros 0-9
	if(objeto.val().match(/^[0-9]+$/))
	{
		DivError.html("");
		objeto.css("background-color", "#ccffcc");
		return true;
	}
	else
	{
		DivError.html("<font color='#d59392'>Debe ingresar un numero entero mayor o igual a 0</font>");
		objeto.css("background-color", '#d59392');
		return false;
	}
}
//NumeroEntero: verifica que el campo contenga un numero entero
$.fn.NumeroEntero=function(objeto, DivError)
{
	//empieza y termina con numeros 0-9
	if(objeto.val().match(/^[-]?[0-9]+$/))
	{
		DivError.html("");
		objeto.css("background-color", "#ccffcc");
		return true;
	}
	else
	{
		DivError.html("<font color='#d59392'>Debe ingresar un numero entero</font>");
		objeto.css("background-color", '#d59392');
		return false;
	}
}
//Fecha: verifica que la fecha este en formato 2010-02-23 y sea valida. no considera años biciestos, permite 31 dias en todos los meses
$.fn.Fecha_aaaa_mm_dd=function(objeto, DivError)
{
	//empieza con 4 numeros 0000-9999, sigue un guion, sigue 2 numeros 01-12, un guion, termina con 2 numeros 01-31
	if(objeto.val().match(/^[0-9]{4}[-]{1}(([0]{1}[1-9]{1})|([1]{1}[012]{1})){1}[-]{1}(([0]{1}[1-9]{1})|([12]{1}[0-9]{1}){1}|([3]{1}[01]{1}){1}){1}$/))
	{
		DivError.html("");
		objeto.css("background-color", "#ccffcc");
		return true;
	}
	else
	{
		DivError.html("<font color='#d59392'>Fecha invalida</font>");
		objeto.css("background-color", '#d59392');
		return false;
	}
}
//FechaMayor: determina si la fecha 1 es mayor que la fecha 2
$.fn.FechaMayor=function(fecha1, fecha2)
{
	//empieza con 4 numeros 0000-9999, sigue un guion, sigue 2 numeros 01-12, un guion, termina con 2 numeros 01-31
	if(Date.parse(fecha1)>Date.parse(fecha2))
	{
		return true;
	}
	else
	{
		return false;
	}
}
//Moneda: verifica que sea una moneda
$.fn.Monedas=function(objeto, DivError)
{
	//empieza con 1-9, siguen 0 o mas numeros 0-9, debe terminar con punto y 2 numeros o con numero opcional 0-9
	if(objeto.val().match(/^[1-9]{1}[0-9]{0,}(([.]{1}[0-9]{1,2}$)|([0-9]?$)){1}/))
	{
		DivError.html("");
		objeto.css("background-color", "#ccffcc");
		return true;
	}
	else
	{
		DivError.html("<font color='#d59392'>Moneda invalida</font>");
		objeto.css("background-color", '#d59392');
		return false;
	}
}
//Decimales: verifica que se ingrese un decimal separado por coma
$.fn.Decimales=function(objeto, DivError)
{
	//empieza con 0-9, siguen 0 o mas numeros 0-9, debe terminar con punto y 3 numeros o con numero opcional 0-9
	if(objeto.val().match(/^[0-9]{1,}(([,]{1}[0-9]{1,3}$)|([0-9]?$)){1}/))
	{
		DivError.html("");
		objeto.css("background-color", "#ccffcc");
		return true;
	}
	else
	{
		DivError.html("<font color='#d59392'>Decimal invalido, maximo 3 decimales<br>separe enteros de decimales con una coma(,)</font>");
		objeto.css("background-color", '#d59392');
		return false;
	}
}
//coseno: devuelve el coseno recibiendo grados
function coseno(grados)
{
	return Math.cos(grados * Math.PI / 180);
}
//seno: devuelve el seno recibiendo grados
function seno(grados)
{
	return Math.sin(grados * Math.PI / 180);
}