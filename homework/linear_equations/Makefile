all: Out.txt Out.times.txt Out.times.gnuplot.svg

Out.times.gnuplot.svg : Out.times.txt Makefile
	echo '\
			set terminal svg background "white" ;\
			set out "$@" ;\
			set xlabel "Size of square matrix" ;\
			set ylabel "QR-decomp. runtime (s)" ;\
			set grid ;\
			set title "QR-decomp. runtime vs. square matrix size" ;\
			plot "$<" with lines notitle ;\
		' | gnuplot

Out.txt : main.exe
	mono $< > $@

Out.times.txt : resources.exe
	>$@
	for N in $$(seq 100 5 200); do \
		time --format "$$N %e" --output $@ --append \
		mono $< -size:$$N >> $@;\
	done

main.exe : main.cs vector.dll matrix.dll qrgs.dll
	mcs -target:exe -out:$@ -reference:vector.dll -reference:matrix.dll -reference:qrgs.dll $<

resources.exe : resources.cs vector.dll matrix.dll qrgs.dll
	mcs -target:exe -out:$@ -reference:vector.dll -reference:matrix.dll -reference:qrgs.dll $<

matrix.dll : matrix.cs vector.dll
	mcs -target:library -reference:vector.dll -out:$@ $<

vector.dll : vector.cs
	mcs -target:library -out:$@ $<

qrgs.dll : qrgs.cs matrix.dll vector.dll
	mcs -target:library -reference:matrix.dll -reference:vector.dll -out:$@ $<

clean :
	$(RM) *.exe Out.* *.dll *.svg