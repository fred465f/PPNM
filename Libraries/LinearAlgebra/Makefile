all: matrix.dll vector.dll qrgs.dll

matrix.dll : matrix.cs vector.dll
	mcs -target:library -reference:vector.dll -out:$@ $<

vector.dll : vector.cs
	mcs -target:library -out:$@ $<

qrgs.dll : qrgs.cs matrix.dll vector.dll
	mcs -target:library -reference:matrix.dll -reference:vector.dll -out:$@ $<

clean :
	$(RM) *.dll