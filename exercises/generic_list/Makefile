Out.txt : main.exe
	mono main.exe -input:input.txt > Out.txt

main.exe : main.cs genlist.dll
	mcs -target:exe -reference:genlist.dll -out:main.exe main.cs

genlist.dll : genlist.cs
	mcs -target:library -out:genlist.dll genlist.cs

clean :
	$(RM) main.exe Out.txt genlist.dll
