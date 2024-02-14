Out.txt : main.exe
	mono $< > $@

main.exe : main.cs matrix.dll
	mcs -target:exe -out:$@ -reference:matrix.dll $<

matrix.dll : matrix.cs
	mcs -target:library -out:$@ $<

clean :
	$(RM) main.exe Out.txt matrix.dll
