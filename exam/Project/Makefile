all : Out.resourcesSVD.gnuplot.png Out.testSVD.txt

# Paths to self-made libraries.
LIBSPATH1 = ../../Libraries/LinearAlgebra/

# Images.
Out.resourcesSVD.gnuplot.png : Out.resourcesSVD.data.txt
	echo '\
			set terminal pngcairo background "white" ;\
			set out "$@" ;\
			set xlabel "Size of square matrix" ;\
			set ylabel "SVD runtime (s)" ;\
			set grid ;\
			set key box top left width 1 height 1  ;\
			set title "SVD runtime vs. square matrix size" ;\
			f(x) = b + (x/a)**c ;\
			b = 0.01; a = 1; c = 3 ;\
			fit f(x) "$<" via a, b ;\
			plot "$<" with lines lw 3 linecolor rgb "#4BB3E1" title "Data", \
					f(x) with lines lw 3 dashtype 2 linecolor rgb "#050DE1435" title sprintf("Fit: (x/%.f)^{%.2f} + %.3f", a, c, b) ;\
		' | gnuplot

# Data.
Out.resourcesSVD.data.txt : resourcesSVD.exe
	for N in $$(seq 10 5 180); do \
		time -ao log.$$N -f "$$N %e" mono $< -size:$$N & \
	done; wait
	>$@
	for N in $$(seq 10 5 180); do \
		cat log.$$N >> $@; \
	done
	$(RM) log.*

# Main commandline output.
Out.testSVD.txt : testSVD.exe
	mono $< > $@

# Executables.
testSVD.exe : testSVD.cs Library.dll
	mcs -target:exe -out:$@ -reference:Library.dll $<
resourcesSVD.exe : resourcesSVD.cs Library.dll
	mcs -target:exe -out:$@ -reference:Library.dll $<

# Libraries.
Library.dll : $(LIBSPATH1)Matrix.cs $(LIBSPATH1)Vector.cs $(LIBSPATH1)QRGS.cs SVD.cs
	mcs -target:library -out:$@ $^

# Rule to clean.
clean : 
	$(RM) *.exe [Oo]ut.* *.dll log.*