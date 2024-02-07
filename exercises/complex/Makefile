Out.txt : main.exe
	mono main.exe > Out.txt

cmath.dll : complex.cs cmath.cs
	mcs -target:library -out:cmath.dll $^

main.exe : main.cs cmath.dll
	mcs -target:exe -out:main.exe -reference:cmath.dll main.cs

clean :
	$(RM) main.exe Out.txt
