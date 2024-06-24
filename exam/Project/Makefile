all : Out.testSVD.txt Out.resourcesSVD.gnuplot.png

# Path to external self-made library.
LIBSPATH1 = ../../Libraries/LinearAlgebra/

# Image.
Out.resourcesSVD.gnuplot.png : Out.resourcesSVD.data.txt
	echo '\
			set terminal pngcairo background "white" ;\
			set out "$@" ;\
			set xlabel "Size of square matrix" ;\
			set ylabel "SVD runtime (s)" ;\
			set grid ;\
			set key box top left width 1 height 1  ;\
			set title "SVD runtime vs. square matrix size (without using multiprocessing)" ;\
			f(x) = b + (x/a)**c ;\
			b = 0.05; a = 162; c = 3 ;\
			fit f(x) "$<" via a, b, c ;\
			plot "$<" with lines lw 3 linecolor rgb "#4BB3E1" title "Data", \
					f(x) with lines lw 3 dashtype 2 linecolor rgb "#050DE1435" title sprintf("Fit: (x/%.f)^{%.2f} + %.3f", a, c, b) ;\
		' | gnuplot

# Data.
Out.resourcesSVD.data.txt : resourcesSVD.exe
	>$@
	for N in $$(seq 10 5 150); do \
		mono $< -size:$$N >> $@;\
	done

# Main commandline output. Implementation avoids race conditions even though it should not be a prominent problem for five threads.
Out.testSVD.txt : testSVD.exe
	for N in $$(seq 1 1 5); do \
		mono $< -testNumber:$$N >> log.$$N & \
	done; wait
	>$@
	for N in $$(seq 1 1 5); do \
		cat log.$$N >> $@; \
	done
	$(RM) log.*

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