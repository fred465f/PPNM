Out.txt : main.exe
	mono $< > $@

main.exe : main.cs vector.dll matrix.dll
	mcs -target:exe -out:$@ -reference:vector.dll -reference:matrix.dll $<

matrix.dll : matrix.cs
	mcs -target:library -out:$@ $<

vector.dll : vector.cs matrix.dll
	mcs -target:library -reference:matrix.dll -out:$@ $<

clean :
	$(RM) main.exe Out.txt matrix.dll vector.dll
